using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using WorkTimer.MediatR.Handlers.Shared;
using WorkTimer.MediatR.Requests;
using WorkTimer.Persistence.Data;

namespace WorkTimer.MediatR.Handlers
{
    public class DeleteWorkingPeriodHandler : TotalHoursBase, IRequestHandler<DeleteWorkingPeriodRequest, bool>
    {
        private readonly ILogger<DeleteWorkingPeriodHandler> _logger;

        public DeleteWorkingPeriodHandler(AppDbContext context, ILogger<DeleteWorkingPeriodHandler> logger) : base(context)
        {
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

                var workDay = await _context.WorkDays.Include(x => x.WorkingPeriods).FirstOrDefaultAsync(x => x.WorkingPeriods.Any(wp => wp.WorkDayId == request.WorkingPeriod.WorkDayId));
                workDay.TotalHours = CalculateTotalHoursFromWorkDay(workDay);

                await _context.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Working period with id {request?.WorkingPeriod.Id} of User '{request?.User?.Id}' could not be deleted");

                return false;
            }
        }
    }
}