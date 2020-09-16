using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
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
                var workingPeriods = _context.WorkingPeriods.Where(a => a.WorkDayId == request.WorkDay.Id).Select(x => new { x.StartTime.Date }).ToList();
                if (workingPeriods?.Count > 0) {
                    var count = workingPeriods.GroupBy(x => x.Date).Count();

                    if (count == 1) {
                        var date = workingPeriods.FirstOrDefault().Date;
                        if (date != request.WorkDay.Date) {
                            request.WorkDay.Date = date;
                        }
                    }
                }

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
