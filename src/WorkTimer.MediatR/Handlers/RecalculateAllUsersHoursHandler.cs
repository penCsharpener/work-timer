using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using WorkTimer.MediatR.Handlers.Shared;
using WorkTimer.MediatR.Requests;
using WorkTimer.Persistence.Data;

namespace WorkTimer.MediatR.Handlers {
    public class RecalculateAllUsersHoursHandler : TotalHoursBase, IRequestHandler<RecalculateAllUsersHoursRequest, string> {
        private readonly ILogger<RecalculateAllUsersHoursHandler> _logger;

        public RecalculateAllUsersHoursHandler(AppDbContext context, ILogger<RecalculateAllUsersHoursHandler> logger) : base(context) {
            _logger = logger;
        }

        public Task<string> Handle(RecalculateAllUsersHoursRequest request, CancellationToken cancellationToken) {
            var workdays = _context.WorkDays.Include(x => x.WorkingPeriods).ToList();

            foreach (var workDay in workdays) {
                UpdateTotalHoursOfWorkDay(workDay);
            }

            _context.SaveChanges();

            _logger.LogInformation($"Processed {workdays.Count} work days.");
            return Task.FromResult($"Processed {workdays.Count} work days.");
        }
    }
}
