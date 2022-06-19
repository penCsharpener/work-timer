using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;
using WorkTimer.Domain.Models;
using WorkTimer.MediatR.Handlers.Shared;
using WorkTimer.MediatR.Models;
using WorkTimer.Messaging.Abstractions;
using WorkTimer.Persistence.Data;

namespace WorkTimer.MediatR.Handlers;

public class DeleteWorkingPeriodRequest : UserContext, IRequest<bool>
{
    public DeleteWorkingPeriodRequest(WorkingPeriod workingPeriod)
    {
        WorkingPeriod = workingPeriod;
    }

    public WorkingPeriod WorkingPeriod { get; set; }
}

public class DeleteWorkingPeriodHandler : TotalHoursBase, IRequestHandler<DeleteWorkingPeriodRequest, bool>
{
    private readonly IMessageService _messageService;
    private readonly ILogger<DeleteWorkingPeriodHandler> _logger;

    public DeleteWorkingPeriodHandler(AppDbContext context, IMessageService messageService, ILogger<DeleteWorkingPeriodHandler> logger) : base(context)
    {
        _messageService = messageService;
        _logger = logger;
    }

    public async Task<bool> Handle(DeleteWorkingPeriodRequest request, CancellationToken cancellationToken)
    {
        try
        {
            if (request.WorkingPeriod == null)
            {
                return false;
            }

            _context.WorkingPeriods.Remove(request.WorkingPeriod);

            await _context.SaveChangesAsync();
            await _messageService.UpdateTotalHoursFromWorkDayAsync(request.WorkingPeriod.WorkDayId);

            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Working period with id {request?.WorkingPeriod.Id} of User '{request?.User?.Id}' could not be deleted");

            return false;
        }
    }
}