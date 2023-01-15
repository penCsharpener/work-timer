using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using WorkTimer.Domain.Extensions;
using WorkTimer.Domain.Models;
using WorkTimer.MediatR.Handlers.Shared;
using WorkTimer.MediatR.Models;
using WorkTimer.MediatR.Services.Abstractions;
using WorkTimer.Messaging.Abstractions;
using WorkTimer.Persistence.Data;

namespace WorkTimer.MediatR.Handlers;

public class NewWorkingPeriodRequest : UserContext, IRequest<EmptyResult>
{
    public string Comment { get; set; }
}

public class NewWorkingPeriodHandler : TotalHoursBase, IRequestHandler<NewWorkingPeriodRequest, EmptyResult>
{
    private readonly IMessageService _messageService;
    private readonly INow _now;
    private readonly ILogger<NewWorkingPeriodHandler> _logger;
    private WorkDay _workDayToday;

    public NewWorkingPeriodHandler(AppDbContext context, IMessageService messageService, INow now, ILogger<NewWorkingPeriodHandler> logger) : base(context)
    {
        _messageService = messageService;
        _now = now;
        _logger = logger;
    }

    public async Task<EmptyResult> Handle(NewWorkingPeriodRequest request, CancellationToken cancellationToken)
    {
        if (request.CurrentContract == null)
        {
            throw new ArgumentNullException(nameof(Contract));
        }

        var unfinishedWorkingPeriod = await _context.WorkingPeriods.FirstOrDefaultAsync(x => x.WorkDay.Contract.UserId == request.User.Id && x.WorkDay.Contract.IsCurrent && x.EndTime == null);

        if (unfinishedWorkingPeriod is not null)
        {
            unfinishedWorkingPeriod.EndTime = _now.Now;
            return await SaveChangesAndRecalculateStatsAsync(request.User.Id);
        }

        var workDayToday = await _context.WorkDays.Include(x => x.WorkingPeriods)
                                                  .Include(x => x.Contract)
                                                  .FirstOrDefaultAsync(x => x.Contract.UserId == request.User.Id && x.Contract.IsCurrent && x.Date == _now.Now.Date);

        workDayToday ??= CreateWorkday(request.CurrentContract);

        workDayToday.WorkingPeriods.Add(new() { Comment = request.Comment, StartTime = _now.Now });
        workDayToday.RequiredHours = workDayToday.GetRequiredHoursForDay(request.CurrentContract.HoursPerWeek);

        return await SaveChangesAndRecalculateStatsAsync(request.User.Id);
    }

    private async Task<EmptyResult> SaveChangesAndRecalculateStatsAsync(int userId)
    {
        await _context.SaveChangesAsync();
        await _messageService.RecalculateStatsAsync(userId);

        return EmptyResult.Empty;
    }

    private WorkDay CreateWorkday(Contract currentContract)
    {
        if (currentContract == null)
        {
            throw new ArgumentNullException(nameof(Contract));
        }

        _workDayToday = new WorkDay { ContractId = currentContract.Id, Date = _now.Now.Date, WorkDayType = _now.Now.ToWorkDayType(), WorkingPeriods = new List<WorkingPeriod>() };
        _workDayToday.RequiredHours = _workDayToday.GetRequiredHoursForDay(currentContract.HoursPerWeek);
        _context.WorkDays.Add(_workDayToday);

        return _workDayToday;
    }
}