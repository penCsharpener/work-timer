using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
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

            var results = _context.WorkDays.Include(x => x.Contract).Include(x => x.WorkingPeriods)
                .Where(x => x.Contract.UserId == request.User.Id)
                .ToList()
                .Select(x => new DisplayWorkDayModel(x) {
                    OverhoursInSeconds = x.WorkingPeriods.Where(y => y.EndTime.HasValue).Sum(y => (y.EndTime.Value - y.StartTime).TotalSeconds),
                    HasOngoingWorkingDay = x.WorkingPeriods.Any(y => !y.EndTime.HasValue),
                    ContractedHours = x.Contract.HoursPerWeek / 5
                })
                .ToList();

            var mostRecent = _context.WorkingPeriods.Include(x => x.WorkDay).ThenInclude(x => x.Contract)
                .Where(x => x.WorkDay.Contract.UserId == request.User.Id)
                .OrderByDescending(x => x.StartTime)
                .Take(5)
                .ToList();

            return Task.FromResult(new IndexResponse {
                WorkDays = results.Select(x => x).ToList(),
                TotalOverHours = TimeSpan.FromSeconds(results.Sum(x => x.Overhours.TotalSeconds)),
                MostRecentWorkPeriods = mostRecent
            });
        }
    }
}
