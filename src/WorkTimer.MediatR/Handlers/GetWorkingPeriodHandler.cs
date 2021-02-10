using MediatR;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using WorkTimer.Domain.Models;
using WorkTimer.MediatR.Models;
using WorkTimer.MediatR.Requests;
using WorkTimer.MediatR.Responses;
using WorkTimer.Persistence.Data;

namespace WorkTimer.MediatR.Handlers {
    public class GetWorkingPeriodHandler : IRequestHandler<GetWorkingPeriodRequest, GetWorkingPeriodResponse> {
        private readonly AppDbContext _context;
        private readonly ILogger<GetWorkingPeriodHandler> _logger;

        public GetWorkingPeriodHandler(AppDbContext context, ILogger<GetWorkingPeriodHandler> logger) {
            _context = context;
            _logger = logger;
        }

        public Task<GetWorkingPeriodResponse> Handle(GetWorkingPeriodRequest request, CancellationToken cancellationToken) {
            WorkingPeriod? workingPeriod = _context.WorkingPeriods.Where(x => x.WorkDayId == request.WorkDayId && x.Id == request.WorkingPeriodId)
                .SingleOrDefault();

            if (workingPeriod != null) {
                return Task.FromResult(new GetWorkingPeriodResponse {
                    WorkingPeriod = workingPeriod, UserContext = new UserContext { User = request.User, UserEmail = request.UserEmail, UserIsAdmin = request.UserIsAdmin }
                });
            }

            return Task.FromResult(default(GetWorkingPeriodResponse));
        }
    }
}