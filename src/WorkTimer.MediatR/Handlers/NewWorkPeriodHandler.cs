using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Nager.Date;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using WorkTimer.Domain.Models;
using WorkTimer.MediatR.Handlers.Shared;
using WorkTimer.MediatR.Requests;
using WorkTimer.MediatR.Services.Abstractions;
using WorkTimer.Persistence.Data;
using static Nager.Date.DateSystem;

namespace WorkTimer.MediatR.Handlers
{
    public class NewWorkPeriodHandler : TotalHoursBase, IRequestHandler<NewWorkPeriodRequest, bool>
    {
        private readonly INow _now;
        private readonly ILogger<NewWorkPeriodHandler> _logger;
        private WorkDay _workDayToday;

        public NewWorkPeriodHandler(AppDbContext context, INow now, ILogger<NewWorkPeriodHandler> logger) : base(context)
        {
            _now = now;
            _logger = logger;
        }

        public Task<bool> Handle(NewWorkPeriodRequest request, CancellationToken cancellationToken)
        {
            try
            {
                _workDayToday = _context.WorkDays.Include(x => x.WorkingPeriods).Include(x => x.Contract)
                .Where(x => (x.Contract.UserId == request.User.Id && x.Contract.IsCurrent) && x.Date == _now.Now.Date || x.WorkingPeriods.Any(wp => !wp.EndTime.HasValue))
                .FirstOrDefault();

                if (_workDayToday != null && HasOngoingWorkingPeriod())
                {
                    return Task.FromResult(true);
                }

                if (!GetOrCreateWorkday(request.User.Id))
                {
                    return Task.FromResult(false);
                }

                _context.WorkingPeriods.Add(new WorkingPeriod { Comment = request.Comment, StartTime = _now.Now, WorkDayId = _workDayToday.Id });

                UpdateTotalHoursOfWorkDay(_workDayToday);

                _context.SaveChanges();

                return Task.FromResult(true);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "new work period could not be created");

                return Task.FromResult(false);
            }
        }

        private bool GetOrCreateWorkday(int userId)
        {
            if (_workDayToday == null)
            {
                Contract? contract = _context.Contracts.Where(x => x.UserId == userId && x.IsCurrent).FirstOrDefault();

                if (contract == null)
                {
                    return false;
                }



                _workDayToday = new WorkDay { ContractId = contract.Id, Date = _now.Now.Date, WorkDayType = GetWorkdayTypeToday(_now) };
                _context.WorkDays.Add(_workDayToday);
                _context.SaveChanges();
            }

            return true;
        }

        private bool HasOngoingWorkingPeriod()
        {
            WorkingPeriod? unfinished = _workDayToday.WorkingPeriods?.FirstOrDefault(x => !x.EndTime.HasValue);

            if (unfinished != null)
            {
                unfinished.EndTime = _now.Now;

                _context.WorkingPeriods.Update(unfinished);
                _context.SaveChanges();

                return true;
            }

            return false;
        }

        public static WorkDayType GetWorkdayTypeToday(INow now)
        {
            if (IsPublicHoliday(now.Now.Date, CountryCode.DE, "DE-SN"))
            {
                return WorkDayType.BankHoliday;
            }

            if (IsWeekend(now.Now, CountryCode.DE))
            {
                return WorkDayType.Weekend;
            }

            return WorkDayType.Workday;
        }
    }
}