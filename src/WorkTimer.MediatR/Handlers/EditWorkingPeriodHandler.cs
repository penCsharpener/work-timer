using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;
using WorkTimer.MediatR.Responses;
using WorkTimer.Persistence.Data;

namespace WorkTimer.MediatR.Handlers {
    public class EditWorkingPeriodHandler : IRequestHandler<GetWorkingPeriodResponse, bool> {
        private readonly AppDbContext _context;
        private readonly ILogger<EditWorkingPeriodHandler> _logger;

        public EditWorkingPeriodHandler(AppDbContext context, ILogger<EditWorkingPeriodHandler> logger) {
            _context = context;
            _logger = logger;
        }

        public Task<bool> Handle(GetWorkingPeriodResponse request, CancellationToken cancellationToken) {
            try {

                _context.WorkingPeriods.Update(request.WorkingPeriod);

                _context.SaveChanges();

                return Task.FromResult(true);

            } catch (Exception ex) {
                _logger.LogError(ex, "Could not update working period with id " + request.WorkingPeriod.Id);
                return Task.FromResult(false);
            }
        }
    }
}
