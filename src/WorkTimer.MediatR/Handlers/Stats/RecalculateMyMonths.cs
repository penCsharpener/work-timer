using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using WorkTimer.Domain.Extensions;
using WorkTimer.Domain.Models;
using WorkTimer.MediatR.Models;
using WorkTimer.Persistence.Data;

namespace WorkTimer.MediatR.Handlers.Stats;

public class RecalculateMyMonthsRequest : UserContext, IRequest<RecalculateMyMonthsResponse>
{
    private readonly int _year;
    private readonly int _month;

    public RecalculateMyMonthsRequest() { }

    public RecalculateMyMonthsRequest(int year, int month)
    {
        _year = year;
        _month = month;
    }
}

public class RecalculateMyMonthsResponse { }

public class RecalculateMyMonthsHandler : IRequestHandler<RecalculateMyMonthsRequest, RecalculateMyMonthsResponse>
{
    private readonly AppDbContext _context;
    private readonly ILogger<RecalculateMyMonthsHandler> _logger;

    public RecalculateMyMonthsHandler(AppDbContext context, ILogger<RecalculateMyMonthsHandler> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<RecalculateMyMonthsResponse> Handle(RecalculateMyMonthsRequest request, CancellationToken cancellationToken)
    {
        RecalculateMyMonthsResponse response = new();

        try
        {
            var contract = request.User.Contracts.FirstOrDefault(x => x.IsCurrent);
            var existingMonths = await _context.WorkMonths.Where(x => x.ContractId == request.CurrentContract.Id).ToListAsync(cancellationToken);
            var workDays = await _context.WorkDays.Where(x => x.ContractId == contract.Id).ToListAsync(cancellationToken);

            AssignExistingWorkMonths(workDays, existingMonths);
            CreateMissingWorkMonths(workDays, existingMonths, request.User);

            await _context.SaveChangesAsync(cancellationToken);

            UpdateStats(workDays, existingMonths, contract);

            await _context.SaveChangesAsync(cancellationToken);

        }
        catch (System.Exception ex)
        {
            _logger.LogError(ex, "Failed to update monthly stats.");
        }

        return response;
    }

    private void AssignExistingWorkMonths(List<WorkDay> workDays, List<WorkMonth> existingMonths)
    {
        foreach (var day in workDays)
        {
            if (existingMonths.FirstOrDefault(x => x.Month == day.Date.Month && x.Year == day.Date.Year && x.ContractId == day.ContractId) is { } month)
            {
                day.WorkMonthId = month.Id;
            }
        }
    }

    private void CreateMissingWorkMonths(List<WorkDay> workDays, List<WorkMonth> existingMonths, AppUser user)
    {
        foreach (var day in workDays)
        {
            var matchingMonth = existingMonths.FirstOrDefault(x => x.Month == day.Date.Month && x.Year == day.Date.Year);

            if (matchingMonth == null)
            {
                WorkMonth month = new()
                {
                    ContractId = day.ContractId,
                    Year = day.Date.Year,
                    Month = day.Date.Month,
                };

                _context.WorkMonths.Add(month);
                existingMonths.Add(month);

                day.WorkMonth = month;
            }
            else
            {
                day.WorkMonth = matchingMonth;
            }
        }
    }

    private void UpdateStats(List<WorkDay> workDays, List<WorkMonth> existingMonths, Contract contract)
    {
        foreach (var month in existingMonths)
        {
            var days = workDays.Where(x => x.WorkMonthId == month.Id).ToList();

            month.TotalHours = days.Sum(x => x.TotalHours);
            month.TotalOverhours = days.Sum(x => x.GetOverhours());
            month.DaysWorked = days.Count(x => x.WorkDayType == WorkDayType.Workday);
            month.DaysOffWork = DateTimeExtensions.GetTotalDaysInMonth(month.Month) - month.DaysWorked;
        }
    }
}
