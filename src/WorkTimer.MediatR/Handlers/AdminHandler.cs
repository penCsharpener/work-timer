using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using WorkTimer.MediatR.Requests;
using WorkTimer.MediatR.Responses;

namespace WorkTimer.MediatR.Handlers {
    public class AdminHandler : IRequestHandler<AdminRequest, AdminResponse> {
        private readonly ILogger<AdminHandler> _logger;
        private readonly IMediator _mediator;

        public AdminHandler(IMediator mediator, ILogger<AdminHandler> logger) {
            _mediator = mediator;
            _logger = logger;
        }

        public async Task<AdminResponse> Handle(AdminRequest request, CancellationToken cancellationToken) {
            try {
                StringBuilder messageText = new StringBuilder();

                if (request.CalculateZeroHourWorkDays) {
                    messageText.Append(await _mediator.Send(new CalculateZeroHourWorkDaysRequest()));
                }

                if (request.RecalculateAllMyWorkDays) {
                    messageText.Append(await _mediator.Send(new RecalculateAllMyWorkDaysRequest()));
                }

                if (request.RecalculateAllUsersHours) {
                    messageText.Append(await _mediator.Send(new RecalculateAllUsersHoursRequest()));
                }

                return new AdminResponse { HasError = false, Message = "Batch jobs ran successfully:" + messageText };
            } catch (Exception ex) {
                _logger.LogError(ex, "Could not complete admin request.");

                return AdminResponse.ErrorMessage("An error occurred.");
            }
        }
    }
}