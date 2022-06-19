using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;
using WorkTimer.Domain.Models;
using WorkTimer.MediatR.Models;
using WorkTimer.Persistence.Data;

namespace WorkTimer.MediatR.Handlers;
public class DeleteWorkDayRequest : UserContext, IRequest<bool>
{
    public DeleteWorkDayRequest(WorkDay workDay)
    {
        WorkDay = workDay;
    }

    public WorkDay WorkDay { get; set; }
}

public class DeleteWorkDayHandler : IRequestHandler<DeleteWorkDayRequest, bool>
{
    private readonly AppDbContext _context;
    private readonly ILogger<DeleteWorkDayHandler> _logger;

    public DeleteWorkDayHandler(AppDbContext context, ILogger<DeleteWorkDayHandler> logger)
    {
        _context = context;
        _logger = logger;
    }

    public Task<bool> Handle(DeleteWorkDayRequest request, CancellationToken cancellationToken)
    {
        try
        {
            if (request.WorkDay == null)
            {
                return Task.FromResult(false);
            }

            _context.WorkDays.Remove(request.WorkDay);
            _context.SaveChanges();

            return Task.FromResult(true);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Work day with id {request.WorkDay.Id} of User '{request.User?.Id}' could not be deleted");

            return Task.FromResult(false);
        }
    }
}