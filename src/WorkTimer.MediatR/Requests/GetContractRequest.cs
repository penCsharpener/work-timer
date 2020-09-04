using MediatR;
using WorkTimer.MediatR.Models;
using WorkTimer.MediatR.Responses;

namespace WorkTimer.MediatR.Requests {
    public class GetContractRequest : UserContext, IRequest<GetContractResponse> {
        public GetContractRequest(int contractId) {
            ContractId = contractId;
        }

        public int ContractId { get; set; }
    }
}
