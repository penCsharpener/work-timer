﻿using MediatR;
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

namespace WorkTimer.MediatR.Handlers.Stats
{
    public class RecalculateMyWeeksRequest : UserContext, IRequest<RecalculateMyWeeksResponse>
    {
        private readonly int _year;
        private readonly int _calendarWeek;
        private readonly DateTime _date;

        private static Calendar _calendar = new CultureInfo("de-DE").Calendar;

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

    public class RecalculateMyWeeksResponse
    {

    }

    public class RecalculateMyWeeksHandler : TotalHoursBase, IRequestHandler<RecalculateMyWeeksRequest, RecalculateMyWeeksResponse>
    {
        private readonly ILogger<RecalculateMyWeeksHandler> _logger;

        public RecalculateMyWeeksHandler(AppDbContext context, ILogger<RecalculateMyWeeksHandler> logger) : base(context)
        {
            _logger = logger;
        }

        public async Task<RecalculateMyWeeksResponse> Handle(RecalculateMyWeeksRequest request, CancellationToken cancellationToken)
        {
            var response = new RecalculateMyWeeksResponse();

            try
            {
                if (request.CalendarWeek == 0 && request.DaysInWeek?.Count == 0)
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
            var workDays = await _context.WorkDays.Include(x => x.WorkingPeriods).Include(x => x.WorkWeek).Include(x => x.Contract)
                                                  .Where(x => x.Contract.UserId == request.User.Id)
                                                  .ToListAsync();

            var existingWorkWeeks = await _context.WorkWeeks.Where(x => x.UserId == request.User.Id).ToListAsync();

            var firstDay = workDays.Select(x => x.Date).Min();
            var lastDay = workDays.Select(x => x.Date).Max();

            var firstWeek = firstDay.GetWeekNumber();
            var lastWeek = lastDay.GetWeekNumber();
            var workWeeks = new List<WorkWeek>();

            for (int year = firstWeek; year <= lastWeek; year++)
            {
                for (int w = 0; w <= 52; w++)
                {
                    WorkWeek workWeek;
                    if (existingWorkWeeks.FirstOrDefault(x => x.WeekNumber == w && x.WeekStart.Year == year) is { } existingWeek)
                    {
                        workWeek = existingWeek;
                    }
                    else
                    {
                        DateTimeExtensions.GetWeekNumber(year, w, out var weekStart);

                        workWeek = new WorkWeek
                        {
                            WeekNumber = w,
                            WeekStart = weekStart.GetWholeWeek().First(),
                            UserId = request.User.Id,
                        };
                    }

                    workWeeks.Add(workWeek);
                }
            }

            _context.WorkWeeks.AddRange(workWeeks.Where(x => x.Id == 0));

            await _context.SaveChangesAsync();
        }

        public async Task ProcessSpecificWorkWeek(RecalculateMyWeeksRequest request)
        {
            var workDays = await _context.WorkDays.Include(x => x.WorkingPeriods).Include(x => x.WorkWeek).Include(x => x.Contract)
                                    .Where(new RecalculateMyWeeksSpecification(request.User.Id, request.DaysInWeek).ToExpression())
                                    .ToListAsync();

            var totalHours = workDays.Sum(x => CalculateTotalHoursFromWorkDay(x));
            var totalOverhours = workDays.Sum(x => CalculateTotalOverhoursFromWorkDay(x));
            var daysOffWork = workDays.Count(x => x.WorkDayType != Domain.Models.WorkDayType.Workday && x.WorkDayType != Domain.Models.WorkDayType.Undefined);
            var daysWorked = workDays.Count(x => x.WorkDayType == Domain.Models.WorkDayType.Workday);

            var workWeek = await _context.WorkWeeks.FirstOrDefaultAsync(x => x.UserId == request.User.Id && x.WeekNumber == request.CalendarWeek);

            workWeek.TotalHours = totalHours;
            workWeek.TotalOverhours = totalOverhours;
            workWeek.DaysOffWork = daysOffWork;
            workWeek.DaysWorked = daysWorked;

            await _context.SaveChangesAsync();
        }
    }
}
