using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using WorkTimer.Domain.Extensions;
using WorkTimer.Domain.Models;
using WorkTimer.MediatR.Requests;
using WorkTimer.MediatR.Responses;
using WorkTimer.Persistence.Data;

namespace WorkTimer.MediatR.Handlers {
    public class IndexHandler : IRequestHandler<IndexRequest, IndexResponse> {
        private readonly AppDbContext _context;

        public IndexHandler(AppDbContext context) {
            _context = context;
        }

        public Task<IndexResponse> Handle(IndexRequest request, CancellationToken cancellationToken) {

            if (request.User == null) {
                return Task.FromResult(new IndexResponse());
            }

            var entities = _context.WorkDays.Include(x => x.Contract).Include(x => x.WorkingPeriods)
                .Where(x => x.Contract.UserId == request.User.Id)
                .OrderByDescending(x => x.Date)
                .AsEnumerable();

            var results = MapDisplayModel(entities).ToList();

            var mostRecent = _context.WorkingPeriods.Include(x => x.WorkDay).ThenInclude(x => x.Contract)
                .Where(x => x.WorkDay.Contract.UserId == request.User.Id)
                .OrderByDescending(x => x.StartTime)
                .Take(5)
                .ToList();

            return Task.FromResult(new IndexResponse {
                WorkDays = results,
                TotalOverHours = TimeSpan.FromSeconds(results.Where(x => !x.HasOngoingWorkingDay).Sum(x => x.Overhours.TotalSeconds)),
                MostRecentWorkPeriods = mostRecent,
                HasOngoingWorkPeriod = results.Any(x => x.HasOngoingWorkingDay)
            });
        }

        private IEnumerable<DisplayWorkDayModel> MapDisplayModel(IEnumerable<WorkDay> entities) {
            foreach (var workday in entities) {

                var contractedHours = workday.GetContractedHoursPerDay();
                var secondsWorked = workday.GetWorkTime().TotalSeconds;
                var overHoursSeconds = secondsWorked - (contractedHours * 60 * 60 * workday.GetWorkHourMultiplier());

                yield return new DisplayWorkDayModel(workday) {
                    HasOngoingWorkingDay = workday.WorkingPeriods.Any(y => !y.EndTime.HasValue),
                    ContractedHours = contractedHours,
                    WorkHours = TimeSpan.FromSeconds(secondsWorked),
                    Overhours = TimeSpan.FromSeconds(overHoursSeconds),
                };
            }
        }
    }
}
