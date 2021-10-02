using MediatR;
using Microsoft.EntityFrameworkCore;
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
                var workDay = await _context.WorkDays.FirstOrDefaultAsync(wd => wd.Id == request.WorkDay.Id);
                workDay.RequiredHours = request.WorkDay.GetRequiredHoursForDay(request.CurrentContract.HoursPerWeek);
                workDay.WorkDayType = request.WorkDay.WorkDayType;
                workDay.ContractId = request.WorkDay.ContractId;

                var hasChanges = _context.ChangeTracker.HasChanges();

                if (hasChanges)
                {
                    await _messageService.UpdateOnEditWorkdayAsync(request.WorkDay.Id, request.User.Id);
                }

                await _context.SaveChangesAsync();

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