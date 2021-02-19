using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using WorkTimer.Domain.Extensions;
using WorkTimer.MediatR.Handlers.Shared;
using WorkTimer.MediatR.Responses;
using WorkTimer.Persistence.Data;

namespace WorkTimer.MediatR.Handlers
{
    public class EditWorkDayHandler : TotalHoursBase, IRequestHandler<GetWorkDayDetailsResponse, bool>
    {
        private readonly ILogger<EditWorkDayHandler> _logger;

        public EditWorkDayHandler(AppDbContext context, ILogger<EditWorkDayHandler> logger) : base(context)
        {
            _logger = logger;
        }

        public Task<bool> Handle(GetWorkDayDetailsResponse request, CancellationToken cancellationToken)
        {
            try
            {
                var contract = request.User.Contracts.FirstOrDefault();
                CorrectWorkDayDateBasedOnPeriods(request);
                UpdateTotalHoursOfWorkDay(request.WorkDay);
                request.WorkDay.RequiredHours = request.WorkDay.GetRequiredHoursForDay(contract.HoursPerWeek);

                _context.WorkDays.Update(request.WorkDay);
                _context.SaveChanges();

                return Task.FromResult(true);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Could not update work day with id {request.WorkDay.Id}");

                return Task.FromResult(false);
            }
        }

        private void CorrectWorkDayDateBasedOnPeriods(GetWorkDayDetailsResponse request)
        {
            var workingPeriods = request.WorkDay.WorkingPeriods.Select(x => new { x.StartTime.Date }).ToList();

            if (workingPeriods?.Count > 0)
            {
                int count = workingPeriods.GroupBy(x => x.Date).Count();

                if (count == 1)
                {
                    DateTime date = workingPeriods.FirstOrDefault().Date;

                    if (date != request.WorkDay.Date)
                    {
                        request.WorkDay.Date = date;
                    }
                }
            }
        }
    }
}