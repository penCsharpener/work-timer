using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;
using WorkTimer.MediatR.Responses;
using WorkTimer.Persistence.Data;

namespace WorkTimer.MediatR.Handlers {
    public class EditWorkDayHandler : IRequestHandler<GetWorkDayDetailsResponse, bool> {
        private readonly AppDbContext _context;
        private readonly ILogger<EditWorkDayHandler> _logger;

        public EditWorkDayHandler(AppDbContext context, ILogger<EditWorkDayHandler> logger) {
            _context = context;
            _logger = logger;
        }

        public Task<bool> Handle(GetWorkDayDetailsResponse request, CancellationToken cancellationToken) {
            try {

                _context.WorkDays.Update(request.WorkDay);

                _context.SaveChanges();

                return Task.FromResult(true);

            } catch (Exception ex) {
                _logger.LogError(ex, $"Could not update work day with id {request.WorkDay.Id}");

                return Task.FromResult(false);
            }
        }
    }
}
