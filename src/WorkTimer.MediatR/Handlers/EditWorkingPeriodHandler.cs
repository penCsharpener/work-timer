using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;
using WorkTimer.MediatR.Handlers.Shared;
using WorkTimer.MediatR.Responses;
using WorkTimer.Messaging.Abstractions;
using WorkTimer.Persistence.Data;

namespace WorkTimer.MediatR.Handlers
{
    public class EditWorkingPeriodHandler : TotalHoursBase, IRequestHandler<GetWorkingPeriodResponse, bool>
    {
        private readonly IMessageService _messageService;
        private readonly ILogger<EditWorkingPeriodHandler> _logger;

        public EditWorkingPeriodHandler(AppDbContext context, IMessageService messageService, ILogger<EditWorkingPeriodHandler> logger) : base(context)
        {
            _messageService = messageService;
            _logger = logger;
        }

        public async Task<bool> Handle(GetWorkingPeriodResponse request, CancellationToken cancellationToken)
        {
            try
            {
                _context.WorkingPeriods.Update(request.WorkingPeriod);
                _context.SaveChanges();

                await _messageService.RecalculateStatsAsync(request.UserContext.User.Id);

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Could not update working period with id " + request.WorkingPeriod.Id);

                return false;
            }
        }
    }
}