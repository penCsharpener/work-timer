using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using WorkTimer.Domain.Extensions;
using WorkTimer.MediatR.Handlers.Shared;
using WorkTimer.MediatR.Responses;
using WorkTimer.Messaging.Abstractions;
using WorkTimer.Persistence.Data;

namespace WorkTimer.MediatR.Handlers
{
    public class EditWorkDayHandler : TotalHoursBase, IRequestHandler<GetWorkDayDetailsResponse, bool>
    {
        private readonly IMessageService _messageService;
        private readonly ILogger<EditWorkDayHandler> _logger;

        public EditWorkDayHandler(AppDbContext context, IMessageService messageService, ILogger<EditWorkDayHandler> logger) : base(context)
        {
            _messageService = messageService;
            _logger = logger;
        }

        public async Task<bool> Handle(GetWorkDayDetailsResponse request, CancellationToken cancellationToken)
        {
            try
            {
                var contract = request.User.Contracts.FirstOrDefault();
                CorrectWorkDayDateBasedOnPeriods(request);
                UpdateTotalHoursOfWorkDay(request.WorkDay);
                request.WorkDay.RequiredHours = request.WorkDay.GetRequiredHoursForDay(contract.HoursPerWeek);

                _context.WorkDays.Update(request.WorkDay);
                _context.SaveChanges();
                await _messageService.RecalculateStatsAsync(request.User.Id);

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Could not update work day with id {request.WorkDay.Id}");

                return false;
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