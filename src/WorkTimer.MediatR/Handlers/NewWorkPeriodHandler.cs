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
using WorkTimer.Persistence.Data;
using static Nager.Date.DateSystem;

namespace WorkTimer.MediatR.Handlers {
    public class NewWorkPeriodHandler : TotalHoursBase, IRequestHandler<NewWorkPeriodRequest, bool> {
        private readonly ILogger<NewWorkPeriodHandler> _logger;
        private WorkDay _workDayToday;

        public NewWorkPeriodHandler(AppDbContext context, ILogger<NewWorkPeriodHandler> logger) : base(context) {
            _logger = logger;
        }

        public Task<bool> Handle(NewWorkPeriodRequest request, CancellationToken cancellationToken) {
            try {
                if (!GetOrCreateWorkday(request.User.Id)) {
                    return Task.FromResult(false);
                }

                if (HasOngoingWorkingPeriod()) {
                    return Task.FromResult(true);
                }

                _context.WorkingPeriods.Add(new WorkingPeriod { Comment = request.Comment, StartTime = DateTime.Now, WorkDayId = _workDayToday.Id });

                UpdateTotalHoursOfWorkDay(_workDayToday);

                _context.SaveChanges();

                return Task.FromResult(true);
            } catch (Exception ex) {
                _logger.LogError(ex, "new work period could not be created");

                return Task.FromResult(false);
            }
        }

        private bool GetOrCreateWorkday(int userId) {
            _workDayToday = _context.WorkDays.Include(x => x.WorkingPeriods).Include(x => x.Contract)
                .Where(x => x.Contract.UserId == userId && x.Contract.IsCurrent && x.Date == DateTime.Today)
                .Take(1)
                .FirstOrDefault();

            if (_workDayToday == null) {
                Contract? contract = _context.Contracts.Where(x => x.UserId == userId && x.IsCurrent).FirstOrDefault();

                if (contract == null) {
                    return false;
                }

                _workDayToday = new WorkDay { ContractId = contract.Id, Date = DateTime.Now.Date, WorkDayType = GetWorkdayTypeToday() };
                _context.WorkDays.Add(_workDayToday);
                _context.SaveChanges();
            }

            return true;
        }

        private bool HasOngoingWorkingPeriod() {
            WorkingPeriod? unfinished = _workDayToday.WorkingPeriods?.FirstOrDefault(x => !x.EndTime.HasValue);

            if (unfinished != null) {
                unfinished.EndTime = DateTime.Now;

                _context.WorkingPeriods.Update(unfinished);
                _context.SaveChanges();

                return true;
            }

            return false;
        }

        public static WorkDayType GetWorkdayTypeToday() {
            if (IsPublicHoliday(DateTime.Now.Date, CountryCode.DE, "DE-SN")) {
                return WorkDayType.BankHoliday;
            }

            if (IsWeekend(DateTime.Now, CountryCode.DE)) {
                return WorkDayType.Weekend;
            }

            return WorkDayType.Workday;
        }
    }
}