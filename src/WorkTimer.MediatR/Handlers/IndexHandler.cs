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

namespace WorkTimer.MediatR.Handlers
{
    public partial class IndexHandler : IRequestHandler<IndexRequest, IndexResponse>
    {
        private readonly AppDbContext _context;

        public IndexHandler(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IndexResponse> Handle(IndexRequest request, CancellationToken cancellationToken)
        {
            if (request.User == null || request.CurrentContract == null)
            {
                return new IndexResponse();
            }

            var count = await _context.WorkingPeriods
                .Where(x => x.WorkDay.Contract.UserId == request.User.Id && x.WorkDay.Contract.IsCurrent && x.WorkDay.WorkingPeriods.All(x => x.EndTime.HasValue))
                .CountAsync();

            List<DisplayWorkDayModel> results = MapDisplayModel(request).ToList();

            List<WorkingPeriod> mostRecent = _context.WorkingPeriods.Include(x => x.WorkDay).ThenInclude(x => x.Contract)
                .Where(x => x.WorkDay.Contract.UserId == request.User.Id && x.WorkDay.Contract.IsCurrent)
                .OrderByDescending(x => x.StartTime)
                .Take(5)
                .ToList();

            return new IndexResponse
            {
                WorkDays = new PagedResult<DisplayWorkDayModel>(results, count),
                TotalOverHours = request.CurrentContract?.TotalOverhours ?? TimeSpan.Zero,
                MostRecentWorkPeriods = mostRecent,
                HasOngoingWorkPeriod = results.Any(x => x.HasOngoingWorkingDay)
            };
        }

        private IEnumerable<DisplayWorkDayModel> MapDisplayModel(IndexRequest request)
        {
            var workDays = _context.WorkDays.Include(x => x.Contract).Include(x => x.WorkingPeriods)
                .Where(x => x.Contract.UserId == request.User.Id && x.Contract.IsCurrent)
                .OrderByDescending(x => x.Date)
                .Skip(request.PagingFilter.SkippedItems)
                .Take(request.PagingFilter.PageSize)
                .AsEnumerable();

            foreach (var workDay in workDays)
            {
                double contractedHours = workDay.Contract.GetContractedHoursPerDay();
                double secondsWorked = TimeSpan.FromHours(workDay.TotalHours).TotalSeconds;
                double overHoursSeconds = secondsWorked - (contractedHours * 60 * 60 * workDay.WorkDayType.GetWorkHourMultiplier());

                yield return new DisplayWorkDayModel(workDay)
                {
                    HasOngoingWorkingDay = workDay.WorkingPeriods.Any(y => !y.EndTime.HasValue),
                    ContractedHours = contractedHours,
                    WorkHours = TimeSpan.FromSeconds(secondsWorked),
                    Overhours = TimeSpan.FromSeconds(overHoursSeconds)
                };
            }
        }
    }
}