using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using WorkTimer.MediatR.Requests;
using WorkTimer.MediatR.Responses;
using WorkTimer.Persistence.Data;

namespace WorkTimer.MediatR.Handlers {
    public class GetWorkDayDetailsHandler : IRequestHandler<GetWorkDayDetailsRequest, GetWorkDayDetailsResponse> {
        private readonly AppDbContext _context;
        private readonly ILogger<GetWorkDayDetailsHandler> _logger;

        public GetWorkDayDetailsHandler(AppDbContext context, ILogger<GetWorkDayDetailsHandler> logger) {
            _context = context;
            _logger = logger;
        }
        public Task<GetWorkDayDetailsResponse> Handle(GetWorkDayDetailsRequest request, CancellationToken cancellationToken) {
            var result = _context.WorkDays.Include(x => x.Contract).Include(x => x.WorkingPeriods)
                .Where(x => x.Contract.UserId == request.User.Id && x.Id == request.WorkDayId)
                .FirstOrDefault();

            var contracts = _context.Contracts.Where(x => x.UserId == request.User.Id)
                .Select(x => new ContractDropdownListModel {
                    Id = x.Id,
                    Name = x.Name
                }).ToList();

            if (result != null) {

                return Task.FromResult(
                    new GetWorkDayDetailsResponse {
                        WorkingPeriods = result.WorkingPeriods,
                        WorkDay = result,
                        IsOpenWorkday = result.WorkingPeriods.Any(x => !x.EndTime.HasValue),
                        Contracts = contracts,
                        UserContext = new Models.UserContext {
                            User = request.User,
                            UserEmail = request.UserEmail,
                            UserIsAdmin = request.UserIsAdmin
                        }
                    });
            }

            return null;
        }
    }
}
