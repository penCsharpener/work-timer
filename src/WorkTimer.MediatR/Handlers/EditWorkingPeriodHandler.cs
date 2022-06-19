using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using WorkTimer.Domain.Extensions;
using WorkTimer.Domain.Models;
using WorkTimer.MediatR.Handlers.Shared;
using WorkTimer.Messaging.Abstractions;
using WorkTimer.Persistence.Data;

namespace WorkTimer.MediatR.Handlers;

public class EditWorkingPeriodHandler : TotalHoursBase, IRequestHandler<GetWorkingPeriodResponse, bool>
{
    private readonly IMessageService _messageService;
    private readonly ILogger<EditWorkingPeriodHandler> _logger;
    private int _swappedWorkDayId;
    private int _oldWorkDayId;
    private bool _requiredCreationOfNewWorkDay;

    public EditWorkingPeriodHandler(AppDbContext context, IMessageService messageService, ILogger<EditWorkingPeriodHandler> logger) : base(context)
    {
        _messageService = messageService;
        _logger = logger;
    }

    public async Task<bool> Handle(GetWorkingPeriodResponse request, CancellationToken cancellationToken)
    {
        try
        {
            var workingPeriod = await _context.WorkingPeriods.Include(wp => wp.WorkDay).FirstOrDefaultAsync(wp => wp.Id == request.WorkingPeriod.Id);
            _oldWorkDayId = workingPeriod.WorkDayId;

            if (workingPeriod.StartTime.Date != request.WorkingPeriod.StartTime.Date)
            {
                await SwapWorkday(request, workingPeriod);
            }

            workingPeriod.StartTime = request.WorkingPeriod.StartTime;
            workingPeriod.EndTime = request.WorkingPeriod.EndTime;
            workingPeriod.Comment = request.WorkingPeriod.Comment;

            var hasChanges = _context.ChangeTracker.HasChanges();

            await _context.SaveChangesAsync(cancellationToken);

            if (hasChanges)
            {
                if (_requiredCreationOfNewWorkDay)
                {
                    _swappedWorkDayId = await _context.WorkDays.Where(wd => wd.Date == workingPeriod.StartTime.Date && wd.ContractId == request.UserContext.CurrentContract.Id).Select(wd => wd.Id).FirstOrDefaultAsync(cancellationToken);
                }

                if (_swappedWorkDayId > 0)
                {
                    await _messageService.UpdateTotalHoursFromWorkDayAsync(_swappedWorkDayId);
                }

                await _messageService.UpdateTotalHoursFromWorkDayAsync(_oldWorkDayId);

                await _messageService.RecalculateStatsAsync(request.UserContext.User.Id);
            }

            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Could not update working period with id {WorkingPeriodId}", request.WorkingPeriod.Id);

            return false;
        }
    }

    private async Task SwapWorkday(GetWorkingPeriodResponse request, WorkingPeriod workingPeriod)
    {
        var swapWorkDay = await _context.WorkDays.Include(wd => wd.WorkingPeriods).FirstOrDefaultAsync(wd => wd.Date == request.WorkingPeriod.StartTime.Date && wd.ContractId == request.UserContext.CurrentContract.Id);

        if (swapWorkDay == null)
        {
            _requiredCreationOfNewWorkDay = true;

            WorkDay newWorkDay = new()
            {
                ContractId = request.UserContext.CurrentContract.Id,
                Date = request.WorkingPeriod.StartTime.Date,
                WorkDayType = workingPeriod.StartTime.Date.ToWorkDayType(),
            };

            newWorkDay.RequiredHours = newWorkDay.GetRequiredHoursForDay(request.UserContext.CurrentContract.HoursPerWeek);
            workingPeriod.WorkDay = newWorkDay;
            await _context.WorkDays.AddAsync(newWorkDay);

            return;
        }

        _swappedWorkDayId = swapWorkDay.Id;
        swapWorkDay.WorkingPeriods.Add(workingPeriod);
    }
}