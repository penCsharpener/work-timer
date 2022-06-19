using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using WorkTimer.Domain.Models;
using WorkTimer.MediatR.Models;
using WorkTimer.Persistence.Data;

namespace WorkTimer.MediatR.Handlers;

public class GetWorkingPeriodRequest : UserContext, IRequest<GetWorkingPeriodResponse>
{
    public GetWorkingPeriodRequest(int workDayId, int workingPeriodId)
    {
        WorkDayId = workDayId;
        WorkingPeriodId = workingPeriodId;
    }

    public int WorkDayId { get; set; }
    public int WorkingPeriodId { get; set; }
}

public class GetWorkingPeriodResponse : IRequest<bool>
{
    public WorkingPeriod WorkingPeriod { get; set; }
    public UserContext UserContext { get; set; }

    public DateTime? StartDate { get; set; } = DateTime.Now;
    public TimeSpan? StartTime { get; set; } = DateTime.Now.TimeOfDay;

    public DateTime? EndDate { get; set; }
    public TimeSpan? EndTime { get; set; }
}

public class GetWorkingPeriodHandler : IRequestHandler<GetWorkingPeriodRequest, GetWorkingPeriodResponse>
{
    private readonly AppDbContext _context;
    private readonly ILogger<GetWorkingPeriodHandler> _logger;

    public GetWorkingPeriodHandler(AppDbContext context, ILogger<GetWorkingPeriodHandler> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<GetWorkingPeriodResponse> Handle(GetWorkingPeriodRequest request, CancellationToken cancellationToken)
    {
        var workingPeriod = await _context.WorkingPeriods.Where(x => x.WorkDayId == request.WorkDayId && x.Id == request.WorkingPeriodId)
            .SingleOrDefaultAsync();

        if (workingPeriod != null)
        {
            return new GetWorkingPeriodResponse
            {
                WorkingPeriod = workingPeriod,
                StartDate = workingPeriod.StartTime.Date,
                StartTime = workingPeriod.StartTime.TimeOfDay,
                EndDate = workingPeriod.EndTime?.Date,
                EndTime = workingPeriod.EndTime?.TimeOfDay,
                UserContext = new UserContext { User = request.User, UserEmail = request.UserEmail, UserIsAdmin = request.UserIsAdmin, CurrentContract = request.CurrentContract }
            };
        }

        return default;
    }
}