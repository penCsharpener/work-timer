using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using WorkTimer.Domain.Models;
using WorkTimer.MediatR.Models;
using WorkTimer.MediatR.Requests;
using WorkTimer.MediatR.Responses;
using WorkTimer.Persistence.Data;

namespace WorkTimer.MediatR.Handlers
{
    public class GetWorkingPeriodHandler : IRequestHandler<GetWorkingPeriodRequest, GetWorkingPeriodResponse>
    {
        private readonly AppDbContext _context;
        private readonly ILogger<GetWorkingPeriodHandler> _logger;

        public GetWorkingPeriodHandler(AppDbContext context, ILogger<GetWorkingPeriodHandler> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<GetWorkingPeriodResponse> Handle(GetWorkingPeriodRequest request, CancellationToken cancellationToken)
        {
            WorkingPeriod workingPeriod = await _context.WorkingPeriods.Where(x => x.WorkDayId == request.WorkDayId && x.Id == request.WorkingPeriodId)
                .SingleOrDefaultAsync();

            if (workingPeriod != null)
            {
                return new GetWorkingPeriodResponse
                {
                    WorkingPeriod = workingPeriod,
                    StartDate = workingPeriod.StartTime.Date,
                    StartTime = workingPeriod.StartTime.TimeOfDay,
                    EndDate = workingPeriod.EndTime?.Date,
                    EndTime = workingPeriod.EndTime?.TimeOfDay,
                    UserContext = new UserContext { User = request.User, UserEmail = request.UserEmail, UserIsAdmin = request.UserIsAdmin, CurrentContract = request.CurrentContract }
                };
            }

            return default;
        }
    }
}