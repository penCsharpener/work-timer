using MediatR;
using Microsoft.Extensions.Logging;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using WorkTimer.MediatR.Models;
using WorkTimer.Persistence.Data;

namespace WorkTimer.MediatR.Handlers;

public class GetContractRequest : UserContext, IRequest<GetContractResponse>
{
    public GetContractRequest(int contractId)
    {
        ContractId = contractId;
    }

    public int ContractId { get; set; }
}

public class GetContractResponse : UserContext, IRequest<bool>
{
    public int Id { get; set; }

    [Required]
    public string Name { get; set; }

    [Required]
    public string Employer { get; set; }

    [Required]
    public int HoursPerWeek { get; set; }

    public bool IsCurrent { get; set; }

    public UserContext UserContext { get; set; }
}

public class GetContractHandler : IRequestHandler<GetContractRequest, GetContractResponse>
{
    private readonly AppDbContext _context;
    private readonly ILogger<EditContractHandler> _logger;

    public GetContractHandler(AppDbContext context, ILogger<EditContractHandler> logger)
    {
        _context = context;
        _logger = logger;
    }

    public Task<GetContractResponse> Handle(GetContractRequest request, CancellationToken cancellationToken)
    {
        var contractToEdit = _context.Contracts.Where(x => x.UserId == request.User.Id && x.Id == request.ContractId).FirstOrDefault();

        return Task.FromResult(new GetContractResponse
        {
            Id = contractToEdit.Id,
            Name = contractToEdit.Name,
            Employer = contractToEdit.Employer,
            HoursPerWeek = contractToEdit.HoursPerWeek,
            IsCurrent = contractToEdit.IsCurrent,
            UserContext = request,
        });
    }
}