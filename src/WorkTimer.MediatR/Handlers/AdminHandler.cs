using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using WorkTimer.MediatR.Handlers.Stats;

namespace WorkTimer.MediatR.Handlers;

public class AdminRequest : IRequest<AdminResponse>
{
    public bool CalculateZeroHourWorkDays { get; set; }
    public bool RecalculateAllMyWorkDays { get; set; }
    public bool RecalculateAllUsersHours { get; set; }
    public bool RecalculateAllWorkMonths { get; set; }
}

public class AdminResponse
{
    public bool HasError { get; set; }
    public string Message { get; set; }

    public static AdminResponse ErrorMessage(string message)
    {
        return new AdminResponse { HasError = true, Message = message };
    }
}

public class AdminHandler : IRequestHandler<AdminRequest, AdminResponse>
{
    private readonly ILogger<AdminHandler> _logger;
    private readonly IMediator _mediator;

    public AdminHandler(IMediator mediator, ILogger<AdminHandler> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    public async Task<AdminResponse> Handle(AdminRequest request, CancellationToken cancellationToken)
    {
        try
        {
            StringBuilder messageText = new();

            if (request.CalculateZeroHourWorkDays)
            {
                messageText.Append(await _mediator.Send(new CalculateZeroHourWorkDaysRequest()));
            }

            if (request.RecalculateAllMyWorkDays)
            {
                messageText.Append(await _mediator.Send(new RecalculateAllMyWorkDaysRequest()));
            }

            if (request.RecalculateAllUsersHours)
            {
                messageText.Append(await _mediator.Send(new RecalculateAllUsersHoursRequest()));
            }

            if (request.RecalculateAllWorkMonths)
            {
                await _mediator.Send(new RecalculateMyMonthsRequest());
            }

            return new AdminResponse { HasError = false, Message = "Batch jobs ran successfully:" + messageText };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Could not complete admin request.");

            return AdminResponse.ErrorMessage("An error occurred.");
        }
    }
}