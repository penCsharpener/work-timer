using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using WorkTimer.Domain.Extensions;
using WorkTimer.Domain.Models;
using WorkTimer.MediatR.Handlers.Shared;
using WorkTimer.MediatR.Models;
using WorkTimer.MediatR.Specifications;
using WorkTimer.Persistence.Data;

namespace WorkTimer.MediatR.Handlers.Stats;

public class RecalculateMyWeeksRequest : UserContext, IRequest<RecalculateMyWeeksResponse>
{
    private readonly int _year;
    private readonly int _calendarWeek;
    private readonly DateTime _date;

    private static readonly Calendar _calendar = new CultureInfo("de-DE").Calendar;

    public RecalculateMyWeeksRequest() { }

    public RecalculateMyWeeksRequest(int year, int calendarWeek)
    {
        _year = year;
        _calendarWeek = calendarWeek;

        CalendarWeek = DateTimeExtensions.GetWeekNumber(year, _calendarWeek, out var date);
        DaysInWeek = date.GetWholeWeek().ToList();
    }

    public RecalculateMyWeeksRequest(DateTime date)
    {
        _date = date;

        CalendarWeek = _calendar.GetWeekOfYear(_date, CalendarWeekRule.FirstFullWeek, DayOfWeek.Monday);
        DaysInWeek = _date.GetWholeWeek().ToList();
    }

    public int CalendarWeek { get; }
    public List<DateTime> DaysInWeek { get; }
}

public class RecalculateMyWeeksResponse { }

public class RecalculateMyWeeksHandler : TotalHoursBase, IRequestHandler<RecalculateMyWeeksRequest, RecalculateMyWeeksResponse>
{
    private readonly ILogger<RecalculateMyWeeksHandler> _logger;

    public RecalculateMyWeeksHandler(AppDbContext context, ILogger<RecalculateMyWeeksHandler> logger) : base(context)
    {
        _logger = logger;
    }

    public async Task<RecalculateMyWeeksResponse> Handle(RecalculateMyWeeksRequest request, CancellationToken cancellationToken)
    {
        RecalculateMyWeeksResponse response = new();

        try
        {
            if ((request.CalendarWeek == 0 && request.DaysInWeek == null) || request.DaysInWeek?.Count == 0)
            {
                await ProcessAllWorkWeeks(request);
            }
            else
            {
                await ProcessSpecificWorkWeek(request);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Could not update or create work weeks with parameters CalendarWeek: {request.CalendarWeek} {request.DaysInWeek?.FirstOrDefault().ToString("yyyy-MM-dd HH:mm:ss")}");
        }

        return response;
    }

    public async Task ProcessAllWorkWeeks(RecalculateMyWeeksRequest request)
    {
        var workDays = await _context.WorkDays.Include(x => x.WorkingPeriods).Include(x => x.WorkWeek)
                                              .Where(x => x.ContractId == request.CurrentContract.Id)
                                              .ToListAsync();

        var existingWorkWeeks = await _context.WorkWeeks.Where(x => x.ContractId == request.CurrentContract.Id).ToListAsync();

        var firstDay = workDays.Select(x => x.Date).Min();
        var lastDay = workDays.Select(x => x.Date).Max();

        AssignExistingWorkWeeks(workDays, existingWorkWeeks);
        CreateMissingWorkWeeks(workDays, existingWorkWeeks, request.User);

        await _context.SaveChangesAsync();

        UpdateStats(workDays, existingWorkWeeks, request.User);

        await _context.SaveChangesAsync();
    }

    private void AssignExistingWorkWeeks(IEnumerable<WorkDay> workdays, IEnumerable<WorkWeek> weeks)
    {
        foreach (var day in workdays)
        {
            var firstDay = day.Date.GetWholeWeek().First();
            if (weeks.FirstOrDefault(x => x.WeekStart.Date == firstDay.Date) is { } week)
            {
                day.WorkWeekId = week.Id;
            }
        }
    }

    private void CreateMissingWorkWeeks(IEnumerable<WorkDay> workdays, List<WorkWeek> weeks, AppUser user)
    {
        foreach (var day in workdays)
        {
            var firstDay = day.Date.GetWholeWeek().First();
            var matchingWeek = weeks.FirstOrDefault(x => x.WeekStart.Date == firstDay.Date);
            if (matchingWeek == null)
            {
                WorkWeek week = new()
                {
                    ContractId = user.Id,
                    WeekStart = firstDay,
                    WeekNumber = firstDay.GetWeekNumber(),
                };

                _context.WorkWeeks.Add(week);
                weeks.Add(week);

                day.WorkWeek = week;
            }
            else
            {
                day.WorkWeek = matchingWeek;
            }
        }
    }

    private void UpdateStats(IEnumerable<WorkDay> workdays, List<WorkWeek> weeks, AppUser user)
    {
        var requiredDailyhours = (user.Contracts.FirstOrDefault(x => x.IsCurrent)?.HoursPerWeek ?? 0) / 5d;

        foreach (var week in weeks)
        {
            var days = workdays.Where(x => x.WorkWeekId == week.Id).ToList();

            week.TotalHours = days.Sum(x => x.TotalHours);
            week.TotalOverhours = days.Sum(x => x.TotalHours - requiredDailyhours);
            week.TotalRequiredHours = days.Sum(x => x.RequiredHours);
            week.DaysWorked = days.Count(x => x.WorkDayType == WorkDayType.Workday);
            week.DaysOffWork = 7 - week.DaysWorked;
        }
    }

    public async Task ProcessSpecificWorkWeek(RecalculateMyWeeksRequest request)
    {
        var workDays = await _context.WorkDays.Include(x => x.WorkingPeriods).Include(x => x.WorkWeek).Include(x => x.Contract)
                                .Where(new RecalculateMyWeeksSpecification(request.User.Id, request.DaysInWeek).ToExpression())
                                .ToListAsync();

        var totalHours = workDays.Sum(x => CalculateTotalHoursFromWorkDay(x));
        var totalOverhours = workDays.Sum(x => x.GetOverhours());
        var totalRequiredHours = workDays.Sum(x => x.RequiredHours);
        var daysOffWork = workDays.Count(x => x.WorkDayType != Domain.Models.WorkDayType.Workday && x.WorkDayType != Domain.Models.WorkDayType.Undefined);
        var daysWorked = workDays.Count(x => x.WorkDayType == Domain.Models.WorkDayType.Workday);

        var workWeek = await _context.WorkWeeks.FirstOrDefaultAsync(x => x.ContractId == request.CurrentContract.Id && x.WeekNumber == request.CalendarWeek);

        if (workWeek == null)
        {
            workWeek = new WorkWeek
            {
                WeekNumber = request.CalendarWeek,
                WeekStart = request.DaysInWeek.First(),
                ContractId = request.User.Id
            };

            _context.WorkWeeks.Add(workWeek);
        }

        workWeek.TotalHours = totalHours;
        workWeek.TotalOverhours = totalOverhours;
        workWeek.TotalRequiredHours = totalRequiredHours;
        workWeek.DaysOffWork = daysOffWork;
        workWeek.DaysWorked = daysWorked;

        await _context.SaveChangesAsync();
    }
}
