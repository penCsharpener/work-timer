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
                    var contract = _context.Contracts.Where(x => x.UserId == request.User.Id && x.IsCurrent).FirstOrDefault();

                    if (contract == null) {
                        // some error message and re-route to create a contract
                        contract = new Contract {
                            Employer = "some Employer",
                            Name = "fulltime at some Employer",
                            UserId = request.User.Id,
                            IsCurrent = true,
                            HoursPerWeek = 40,
                        };
                        _context.Contracts.Add(contract);
                        _context.SaveChanges();
                    }

                    workDayToday = new WorkDay {
                        ContractId = contract.Id,
                        Date = DateTime.Now.Date,
                        WorkDayType = WorkDayType.Workday,
                    };
                    _context.WorkDays.Add(workDayToday);
                    _context.SaveChanges();
                }

                WorkDay newWorkDay = null;
                if (workDayToday.Date.Date < DateTime.Now.Date) {
                    newWorkDay = new WorkDay {
                        ContractId = workDayToday.ContractId,
                        Date = DateTime.Now.Date,
                        WorkDayType = WorkDayType.Workday,
                    };

                    _context.WorkDays.Add(newWorkDay);
                    _context.SaveChanges();
                }

                _context.WorkingPeriods.Add(new WorkingPeriod {
                    Comment = request.Comment,
                    StartTime = DateTime.Now,
                    WorkDayId = newWorkDay?.Id ?? workDayToday.Id
                });

                _context.SaveChanges();

                return Task.FromResult(true);
            } catch (System.Exception) {
                return Task.FromResult(false);
            }
        }
    }
}
