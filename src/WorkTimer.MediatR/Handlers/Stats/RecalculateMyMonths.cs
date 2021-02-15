﻿using MediatR;
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

namespace WorkTimer.MediatR.Handlers.Stats
{
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

    public class RecalculateMyMonthsResponse
    {

    }

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
            var response = new RecalculateMyMonthsResponse();

            try
            {
                var contract = request.User.Contracts.FirstOrDefault(x => x.IsCurrent);
                var existingMonths = await _context.WorkMonths.Where(x => x.UserId == request.User.Id).ToListAsync();
                var workDays = await _context.WorkDays.Where(x => x.ContractId == contract.Id).ToListAsync();

                AssignExistingWorkMonths(workDays, existingMonths);
                CreateMissingWorkMonths(workDays, existingMonths, request.User);

                await _context.SaveChangesAsync();

                UpdateStats(workDays, existingMonths, contract);

                await _context.SaveChangesAsync();

            }
            catch (System.Exception ex)
            {

            }

            return response;
        }

        private void AssignExistingWorkMonths(List<WorkDay> workDays, List<WorkMonth> existingMonths)
        {
            foreach (var day in workDays)
            {
                if (existingMonths.FirstOrDefault(x => x.Month == day.Date.Month && x.Year == day.Date.Year) is { } month)
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
                    var month = new WorkMonth
                    {
                        UserId = user.Id,
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
            var requiredDailyhours = (contract?.HoursPerWeek ?? 0) / 5d;

            foreach (var month in existingMonths)
            {
                var days = workDays.Where(x => x.WorkMonthId == month.Id).ToList();

                month.TotalHours = days.Sum(x => x.TotalHours);
                month.TotalOverhours = days.Sum(x => x.TotalHours - requiredDailyhours);
                month.DaysWorked = days.Count(x => x.WorkDayType == WorkDayType.Workday);
                month.DaysOffWork = DateTimeExtensions.GetTotalDaysInMonth(month.Month) - month.DaysWorked;
            }
        }
    }
}
