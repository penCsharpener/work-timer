using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using WorkTimer.Domain.Models;
using WorkTimer.MediatR.Requests;
using WorkTimer.Persistence.Data;
using static Nager.Date.DateSystem;

namespace WorkTimer.MediatR.Handlers {
    public class NewWorkPeriodHandler : IRequestHandler<NewWorkPeriodRequest, bool> {
        private readonly AppDbContext _context;
        private readonly ILogger<NewWorkPeriodHandler> _logger;

        public NewWorkPeriodHandler(AppDbContext context, ILogger<NewWorkPeriodHandler> logger) {
            _context = context;
            _logger = logger;
        }

        public Task<bool> Handle(NewWorkPeriodRequest request, CancellationToken cancellationToken) {
            try {
                var workDayToday = _context.WorkDays.Include(x => x.Contract)
                    .Where(x => x.Contract.UserId == request.User.Id && x.Contract.IsCurrent)
                    .OrderByDescending(x => x.Date)
                    .Take(1)
                    .FirstOrDefault();

                if (workDayToday == null) {
                    var contract = _context.Contracts.Where(x => x.UserId == request.User.Id && x.IsCurrent).FirstOrDefault();

                    if (contract == null) {
                        return Task.FromResult(false);
                    }

                    workDayToday = new WorkDay {
                        ContractId = contract.Id,
                        Date = DateTime.Now.Date,
                        WorkDayType = GetWorkdayTypeToday(),
                    };
                    _context.WorkDays.Add(workDayToday);
                    _context.SaveChanges();
                }

                WorkDay newWorkDay = null;
                if (workDayToday.Date.Date < DateTime.Now.Date) {
                    newWorkDay = new WorkDay {
                        ContractId = workDayToday.ContractId,
                        Date = DateTime.Now.Date,
                        WorkDayType = GetWorkdayTypeToday(),
                    };

                    _context.WorkDays.Add(newWorkDay);
                    _context.SaveChanges();
                }

                var unfinished = workDayToday.WorkingPeriods?.FirstOrDefault(x => !x.EndTime.HasValue);
                if (unfinished != null) {
                    unfinished.EndTime = DateTime.Now;

                    _context.WorkingPeriods.Update(unfinished);
                    _context.SaveChanges();
                    return Task.FromResult(true);
                }

                _context.WorkingPeriods.Add(new WorkingPeriod {
                    Comment = request.Comment,
                    StartTime = DateTime.Now,
                    WorkDayId = newWorkDay?.Id ?? workDayToday.Id
                });

                _context.SaveChanges();

                return Task.FromResult(true);
            } catch (System.Exception ex) {
                _logger.LogError(ex, "new work period could not be created");
                return Task.FromResult(false);
            }
        }

        public static WorkDayType GetWorkdayTypeToday() {
            if (IsPublicHoliday(DateTime.Now.Date, Nager.Date.CountryCode.DE, "DE-SN")) {
                return WorkDayType.BankHoliday;
            }
            if (IsWeekend(DateTime.Now, Nager.Date.CountryCode.DE)) {
                return WorkDayType.Weekend;
            }
            return WorkDayType.Workday;
        }
    }
}
