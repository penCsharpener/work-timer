using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using WorkTimer.Domain.Extensions;
using WorkTimer.Domain.Models;
using WorkTimer.MediatR.Models;
using WorkTimer.Persistence.Data;

namespace WorkTimer.MediatR.Handlers;

public class CreateBlankWorkDayCommand : UserContext, IRequest<CreateBlankWorkDayResponse>
{
    public CreateBlankWorkDayCommand(DateTime date)
    {
        Date = date.Date;
    }

    public DateTime Date { get; set; }
    public WorkDayType WorkDayType { get; set; } = WorkDayType.Undefined;
}

public class CreateBlankWorkDayResponse
{
    public WorkDay WorkDay { get; set; }
}

public class CreateBlankWorkDayHandler : IRequestHandler<CreateBlankWorkDayCommand, CreateBlankWorkDayResponse>
{
    private readonly AppDbContext _context;
    private readonly ILogger<CreateBlankWorkDayHandler> _logger;

    public CreateBlankWorkDayHandler(AppDbContext context, ILogger<CreateBlankWorkDayHandler> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<CreateBlankWorkDayResponse> Handle(CreateBlankWorkDayCommand request, CancellationToken cancellationToken)
    {
        CreateBlankWorkDayResponse response = new();

        try
        {
            var existingWorkDay = await _context.WorkDays.FirstOrDefaultAsync(x => x.Contract.UserId == request.User.Id && x.Contract.IsCurrent && x.Date == request.Date);

            if (existingWorkDay == null)
            {
                var contract = request.User.Contracts.FirstOrDefault();

                existingWorkDay = new WorkDay
                {
                    Date = request.Date,
                    ContractId = contract.Id,
                    RequiredHours = contract.GetContractedHoursPerDay(),
                    WorkDayType = request.WorkDayType == WorkDayType.Undefined ? request.Date.ToWorkDayType() : request.WorkDayType,
                };

                _context.WorkDays.Add(existingWorkDay);
                await _context.SaveChangesAsync();
            }

            response.WorkDay = existingWorkDay;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Could not create workday from date '{request.Date:G}' and user {request.User.Id}");
        }

        return response;
    }
}
