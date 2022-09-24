using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using WorkTimer.Domain.Models;
using WorkTimer.MediatR.Models;
using WorkTimer.Persistence.Data;

namespace WorkTimer.MediatR.Handlers;
public class GetWorkDayDetailsRequest : UserContext, IRequest<GetWorkDayDetailsResponse>
{
    public GetWorkDayDetailsRequest(int workDayId)
    {
        WorkDayId = workDayId;
    }

    public int WorkDayId { get; set; }
}

public class GetWorkDayDetailsResponse : UserContext, IRequest<bool>
{
    public WorkDay WorkDay { get; set; }
    public ICollection<WorkingPeriod> WorkingPeriods { get; set; }
    public List<ContractDropdownListModel> Contracts { get; set; }
    public bool IsOpenWorkday { get; set; }
    public UserContext UserContext { get; set; }
}

public class ContractDropdownListModel
{
    public int Id { get; set; }
    public string Name { get; set; }
}

public class GetWorkDayDetailsHandler : IRequestHandler<GetWorkDayDetailsRequest, GetWorkDayDetailsResponse>
{
    private readonly AppDbContext _context;
    private readonly ILogger<GetWorkDayDetailsHandler> _logger;

    public GetWorkDayDetailsHandler(AppDbContext context, ILogger<GetWorkDayDetailsHandler> logger)
    {
        _context = context;
        _logger = logger;
    }

    public Task<GetWorkDayDetailsResponse> Handle(GetWorkDayDetailsRequest request, CancellationToken cancellationToken)
    {
        var result = _context.WorkDays.Include(x => x.Contract).Include(x => x.WorkingPeriods)
            .Where(x => x.Contract.UserId == request.User.Id && x.Id == request.WorkDayId)
            .FirstOrDefault();

        var contracts = _context.Contracts.Where(x => x.UserId == request.User.Id)
            .Select(x => new ContractDropdownListModel { Id = x.Id, Name = x.Name }).ToList();

        return result != null
            ? Task.FromResult(
                new GetWorkDayDetailsResponse
                {
                    WorkingPeriods = result.WorkingPeriods,
                    WorkDay = result,
                    IsOpenWorkday = result.WorkingPeriods.Any(x => !x.EndTime.HasValue),
                    Contracts = contracts,
                    UserContext = new UserContext { User = request.User, UserEmail = request.UserEmail, UserIsAdmin = request.UserIsAdmin }
                })
            : null;
    }
}