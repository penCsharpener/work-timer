using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using WorkTimer.Domain.Models;
using WorkTimer.MediatR.Models;
using WorkTimer.Persistence.Data;

namespace WorkTimer.MediatR.Handlers;
public class NewContractRequest : UserContext, IRequest<bool>
{
    [Required]
    public string Name { get; set; }

    [Required]
    public string Employer { get; set; }

    [Required]
    public int HoursPerWeek { get; set; }

    public bool IsCurrent { get; set; }
}

public class NewContractHandler : IRequestHandler<NewContractRequest, bool>
{
    private readonly AppDbContext _context;
    private readonly ILogger<NewContractHandler> _logger;

    public NewContractHandler(AppDbContext context, ILogger<NewContractHandler> logger)
    {
        _context = context;
        _logger = logger;
    }

    public Task<bool> Handle(NewContractRequest request, CancellationToken cancellationToken)
    {
        try
        {
            if (request.IsCurrent)
            {
                var currentContracts = _context.Contracts.Where(x => x.UserId == request.User.Id && x.IsCurrent).ToList();

                foreach (var contract in currentContracts)
                {
                    contract.IsCurrent = false;
                }

                _context.SaveChanges();
            }

            Contract newContract = new()
            {
                HoursPerWeek = request.HoursPerWeek,
                UserId = request.User.Id,
                Name = request.Name,
                Employer = request.Employer,
                IsCurrent = request.IsCurrent
            };

            _context.Contracts.Add(newContract);
            _context.SaveChanges();

            return Task.FromResult(true);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Contract couldn't be saved.");

            return Task.FromResult(false);
        }
    }
}