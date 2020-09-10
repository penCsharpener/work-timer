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
                .Where(x => x.Contract.UserId == request.User.Id && x.Contract.IsCurrent)
                .OrderByDescending(x => x.Date)
                .Skip(request.PagingFilter.SkippedItems)
                .Take(request.PagingFilter.PageSize)
                .AsEnumerable();

            var allHours = _context.WorkingPeriods.Include(x => x.WorkDay).ThenInclude(x => x.Contract)
                .Where(x => x.WorkDay.Contract.UserId == request.User.Id && x.WorkDay.Contract.IsCurrent)
                .Select(x => new {
                    WorkDay = new {
                        x.WorkDay.Date,
                        x.WorkDay.WorkDayType,
                        x.WorkDay.Contract.HoursPerWeek
                    },
                    x.StartTime,
                    x.EndTime
                })
                .AsEnumerable();

            var workDayGroups = allHours.GroupBy(x => x.WorkDay);

            var count = workDayGroups.Count();

            var totalOverhours = workDayGroups.Select(x => {
                var secondsWorked = x.Select(w => {
                    if (w.EndTime.HasValue) {
                        return (w.EndTime.Value - w.StartTime).TotalSeconds;
                    }
                    return 0d;
                }).Sum();
                return secondsWorked - ((x.Key.HoursPerWeek / 5d) * 60 * 60 * x.Key.WorkDayType.GetWorkHourMultiplier());
            }).Sum();

            var results = MapDisplayModel(entities).ToList();

            var mostRecent = _context.WorkingPeriods.Include(x => x.WorkDay).ThenInclude(x => x.Contract)
                .Where(x => x.WorkDay.Contract.UserId == request.User.Id && x.WorkDay.Contract.IsCurrent)
                .OrderByDescending(x => x.StartTime)
                .Take(5)
                .ToList();

            return Task.FromResult(new IndexResponse {
                WorkDays = new PagedResult<DisplayWorkDayModel>(results, count),
                TotalOverHours = TimeSpan.FromSeconds(totalOverhours),
                MostRecentWorkPeriods = mostRecent,
                HasOngoingWorkPeriod = results.Any(x => x.HasOngoingWorkingDay)
            });
        }

        private IEnumerable<DisplayWorkDayModel> MapDisplayModel(IEnumerable<WorkDay> entities) {
            foreach (var workday in entities) {

                var contractedHours = workday.GetContractedHoursPerDay();
                var secondsWorked = workday.GetWorkTime().TotalSeconds;
                var overHoursSeconds = secondsWorked - (contractedHours * 60 * 60 * workday.WorkDayType.GetWorkHourMultiplier());

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
