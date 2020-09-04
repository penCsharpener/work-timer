using MediatR;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using WorkTimer.MediatR.Requests;
using WorkTimer.MediatR.Responses;
using WorkTimer.Persistence.Data;

namespace WorkTimer.MediatR.Handlers {
    public class ContractListHandler : IRequestHandler<ContractListRequest, ContractListResponse> {
        private readonly AppDbContext _context;

        public ContractListHandler(AppDbContext context) {
            _context = context;
        }

        public Task<ContractListResponse> Handle(ContractListRequest request, CancellationToken cancellationToken) {
            var result = _context.Contracts.Where(x => x.UserId == request.User.Id).ToList();

            var response = new ContractListResponse(result.Select(x => new ContractListResponse.ContractListModel(x)).ToList()) {
            };

            return Task.FromResult(response);
        }
    }
}
