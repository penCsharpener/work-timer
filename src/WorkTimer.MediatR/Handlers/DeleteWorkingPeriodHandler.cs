using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;
using WorkTimer.MediatR.Requests;
using WorkTimer.Persistence.Data;

namespace WorkTimer.MediatR.Handlers {
    public class DeleteWorkingPeriodHandler : IRequestHandler<DeleteWorkingPeriodRequest, bool> {
        private readonly AppDbContext _context;
        private readonly ILogger<DeleteWorkingPeriodHandler> _logger;

        public DeleteWorkingPeriodHandler(AppDbContext context, ILogger<DeleteWorkingPeriodHandler> logger) {
            _context = context;
            _logger = logger;
        }

        public Task<bool> Handle(DeleteWorkingPeriodRequest request, CancellationToken cancellationToken) {
            try {

                if (request.WorkingPeriod == null) {
                    return Task.FromResult(false);
                }

                _context.WorkingPeriods.Remove(request.WorkingPeriod);
                _context.SaveChanges();

                return Task.FromResult(true);

            } catch (Exception ex) {
                _logger.LogError(ex, $"Working period with id {request.WorkingPeriod} of User '{request.User?.Id}' could not be deleted");
                return Task.FromResult(false);
            }
        }
    }
}
