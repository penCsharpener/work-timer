using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using WorkTimer.Domain.Models;
using WorkTimer.MediatR.Requests;
using WorkTimer.Persistence.Data;

namespace WorkTimer.MediatR.Handlers {
    public class NewWorkPeriodHandler : IRequestHandler<NewWorkPeriodRequest, bool> {
        private readonly AppDbContext _context;

        public NewWorkPeriodHandler(AppDbContext context) {
            _context = context;
        }

        public Task<bool> Handle(NewWorkPeriodRequest request, CancellationToken cancellationToken) {
            try {

                var workDayToday = _context.WorkDays.Include(x => x.Contract)
                    .Where(x => x.Contract.UserId == request.User.Id)
                    .OrderByDescending(x => x.Date)
                    .Take(1)
                    .FirstOrDefault();

                if (workDayToday == null) {

                }

                if (workDayToday.Date.Date < DateTime.Now.Date) {
                    var newWorkDay = new WorkDay {
                        ContractId = workDayToday.ContractId,
                        Date = DateTime.Now.Date,
                        WorkDayType = WorkDayType.Workday,
                    };

                    _context.WorkDays.Add(newWorkDay);
                }

                _context.WorkingPeriods.Add(new WorkingPeriod {
                    Comment = request.Comment,
                    StartTime = DateTime.Now,
                    WorkDayId = workDayToday.Id
                });

                return Task.FromResult(true);
            } catch (System.Exception) {
                return Task.FromResult(false);
            }
        }
    }
}
