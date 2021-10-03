using MediatR;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using WorkTimer.Domain.Models;
using WorkTimer.MediatR.Requests;
using WorkTimer.MediatR.Responses;
using WorkTimer.Persistence.Data;

namespace WorkTimer.MediatR.Handlers {
    public class GetContractHandler : IRequestHandler<GetContractRequest, GetContractResponse> {
        private readonly AppDbContext _context;
        private readonly ILogger<EditContractHandler> _logger;

        public GetContractHandler(AppDbContext context, ILogger<EditContractHandler> logger) {
            _context = context;
            _logger = logger;
        }

        public Task<GetContractResponse> Handle(GetContractRequest request, CancellationToken cancellationToken) {
            Contract? contractToEdit = _context.Contracts.Where(x => x.UserId == request.User.Id && x.Id == request.ContractId).FirstOrDefault();

            return Task.FromResult(new GetContractResponse {
                Id = contractToEdit.Id,
                Name = contractToEdit.Name,
                Employer = contractToEdit.Employer,
                HoursPerWeek = contractToEdit.HoursPerWeek,
                IsCurrent = contractToEdit.IsCurrent
            });
        }
    }
}