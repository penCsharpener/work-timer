﻿using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using WorkTimer.Domain.Extensions;
using WorkTimer.Domain.Models;
using WorkTimer.MediatR.Handlers.Shared;
using WorkTimer.MediatR.Requests;
using WorkTimer.Persistence.Data;

namespace WorkTimer.MediatR.Handlers
{
    public class CalculateZeroHourWorkDaysHandler : TotalHoursBase, IRequestHandler<CalculateZeroHourWorkDaysRequest, string>
    {
        private readonly ILogger<CalculateZeroHourWorkDaysHandler> _logger;

        public CalculateZeroHourWorkDaysHandler(AppDbContext context, ILogger<CalculateZeroHourWorkDaysHandler> logger) : base(context)
        {
            _logger = logger;
        }

        public Task<string> Handle(CalculateZeroHourWorkDaysRequest request, CancellationToken cancellationToken)
        {
            List<WorkDay> zeroHourDays = _context.WorkDays.Include(x => x.Contract).Include(x => x.WorkingPeriods)
                .Where(x => x.TotalHours == 0d)
                .ToList();

            foreach (WorkDay workday in zeroHourDays)
            {
                UpdateTotalHoursOfWorkDay(workday);
                workday.RequiredHours = workday.GetRequiredHoursForDay(workday.Contract.HoursPerWeek);
            }

            _context.SaveChanges();

            _logger.LogInformation($"Processed {zeroHourDays.Count} work days.");

            return Task.FromResult($"Processed {zeroHourDays.Count} work days.");
        }
    }
}