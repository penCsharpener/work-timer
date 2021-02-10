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

            var allHours = _context.WorkingPeriods.Include(x => x.WorkDay).ThenInclude(x => x.Contract)
                .Where(x => x.WorkDay.Contract.UserId == request.User.Id && x.WorkDay.Contract.IsCurrent)
                .Select(x => new { x.WorkDay.Id, x.WorkDay.WorkDayType, x.WorkDay.TotalHours, HoursPerDay = (double)x.WorkDay.Contract.HoursPerWeek / 5d })
                .Distinct()
                .ToList();

            int count = allHours.Count;
            double totalOverHours = allHours.Sum(x => x.TotalHours - (x.HoursPerDay * x.WorkDayType.GetWorkHourMultiplier()));

            List<DisplayWorkDayModel> results = MapDisplayModel(request).ToList();

            List<WorkingPeriod> mostRecent = _context.WorkingPeriods.Include(x => x.WorkDay).ThenInclude(x => x.Contract)
                .Where(x => x.WorkDay.Contract.UserId == request.User.Id && x.WorkDay.Contract.IsCurrent)
                .OrderByDescending(x => x.StartTime)
                .Take(5)
                .ToList();

            return Task.FromResult(new IndexResponse {
                WorkDays = new PagedResult<DisplayWorkDayModel>(results, count),
                TotalOverHours = TimeSpan.FromHours(totalOverHours),
                MostRecentWorkPeriods = mostRecent,
                HasOngoingWorkPeriod = results.Any(x => x.HasOngoingWorkingDay)
            });
        }

        private IEnumerable<DisplayWorkDayModel> MapDisplayModel(IndexRequest request) {
            IEnumerable<WorkDay> entities = _context.WorkDays.Include(x => x.Contract).Include(x => x.WorkingPeriods)
                .Where(x => x.Contract.UserId == request.User.Id && x.Contract.IsCurrent)
                .OrderByDescending(x => x.Date)
                .Skip(request.PagingFilter.SkippedItems)
                .Take(request.PagingFilter.PageSize)
                .AsEnumerable();

            foreach (WorkDay workday in entities) {
                double contractedHours = workday.GetContractedHoursPerDay();
                double secondsWorked = TimeSpan.FromHours(workday.TotalHours).TotalSeconds;
                double overHoursSeconds = secondsWorked - (contractedHours * 60 * 60 * workday.WorkDayType.GetWorkHourMultiplier());

                yield return new DisplayWorkDayModel(workday) {
                    HasOngoingWorkingDay = workday.WorkingPeriods.Any(y => !y.EndTime.HasValue),
                    ContractedHours = contractedHours,
                    WorkHours = TimeSpan.FromSeconds(secondsWorked),
                    Overhours = TimeSpan.FromSeconds(overHoursSeconds)
                };
            }
        }
    }
}