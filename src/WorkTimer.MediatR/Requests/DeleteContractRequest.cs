using MediatR;
using WorkTimer.MediatR.Models;
using WorkTimer.MediatR.Responses;

namespace WorkTimer.MediatR.Requests;

public class DeleteContractRequest : UserContext, IRequest<bool>
{
    public DeleteContractRequest(GetContractResponse contract)
    {
        Contract = contract;
    }

    public GetContractResponse Contract { get; set; }
}
