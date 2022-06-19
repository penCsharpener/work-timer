using MediatR;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using WorkTimer.Domain.Models;
using WorkTimer.MediatR.Models;
using WorkTimer.Persistence.Data;

namespace WorkTimer.MediatR.Handlers;

public class ContractListRequest : UserContext, IRequest<ContractListResponse> { }

public class ContractListResponse
{
    public ContractListResponse(ICollection<ContractListModel> contracts)
    {
        Contracts = contracts;
    }

    public ICollection<ContractListModel> Contracts { get; set; }

    public class ContractListModel : Contract
    {
        public ContractListModel(Contract contract)
        {
            Id = contract.Id;
            Name = contract.Name;
            Employer = contract.Employer;
            HoursPerWeek = contract.HoursPerWeek;
            IsCurrent = contract.IsCurrent;
            UserId = contract.UserId;
        }
    }
}

public class ContractListHandler : IRequestHandler<ContractListRequest, ContractListResponse>
{
    private readonly AppDbContext _context;

    public ContractListHandler(AppDbContext context)
    {
        _context = context;
    }

    public Task<ContractListResponse> Handle(ContractListRequest request, CancellationToken cancellationToken)
    {
        var result = _context.Contracts.Where(x => x.UserId == request.User.Id).ToList();

        ContractListResponse response = new(result.Select(x => new ContractListResponse.ContractListModel(x)).ToList());

        return Task.FromResult(response);
    }
}