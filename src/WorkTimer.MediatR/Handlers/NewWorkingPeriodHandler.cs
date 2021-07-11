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
using WorkTimer.MediatR.Requests;
using WorkTimer.MediatR.Services.Abstractions;
using WorkTimer.Messaging.Abstractions;
using WorkTimer.Persistence.Data;

namespace WorkTimer.MediatR.Handlers
{
    public class NewWorkingPeriodHandler : TotalHoursBase, IRequestHandler<NewWorkingPeriodRequest, bool>
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

        public async Task<bool> Handle(NewWorkingPeriodRequest request, CancellationToken cancellationToken)
        {
            try
            {
                _workDayToday = _context.WorkDays.Include(x => x.WorkingPeriods).Include(x => x.Contract)
                .FirstOrDefault(x => (x.Contract.UserId == request.User.Id && x.Contract.IsCurrent) && x.Date == _now.Now.Date || x.WorkingPeriods.Any(wp => !wp.EndTime.HasValue));

                if (_workDayToday != null && HasOngoingWorkingPeriod())
                {
                    return true;
                }

                if (!GetOrCreateWorkday(request.User.Id))
                {
                    return false;
                }

                _context.WorkingPeriods.Add(new WorkingPeriod { Comment = request.Comment, StartTime = _now.Now, WorkDayId = _workDayToday.Id });

                await _messageService.RecalculateStatsAsync(request.User.Id);
                //UpdateTotalHoursOfWorkDay(_workDayToday);
                _workDayToday.RequiredHours = _workDayToday.GetRequiredHoursForDay(_workDayToday.Contract.HoursPerWeek);

                _context.SaveChanges();

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "new work period could not be created");

                return false;
            }
        }

        private bool GetOrCreateWorkday(int userId)
        {
            if (_workDayToday == null)
            {
                Contract? contract = _context.Contracts.FirstOrDefault(x => x.UserId == userId && x.IsCurrent);

                if (contract == null)
                {
                    return false;
                }

                _workDayToday = new WorkDay { ContractId = contract.Id, Date = _now.Now.Date, WorkDayType = _now.Now.ToWorkDayType() };
                _workDayToday.RequiredHours = _workDayToday.GetRequiredHoursForDay(contract.HoursPerWeek);
                _context.WorkDays.Add(_workDayToday);
                _context.SaveChanges();
            }

            return true;
        }

        private bool HasOngoingWorkingPeriod()
        {
            WorkingPeriod? unfinished = _workDayToday.WorkingPeriods?.FirstOrDefault(x => !x.EndTime.HasValue);

            if (unfinished != null)
            {
                unfinished.EndTime = _now.Now;

                _context.WorkingPeriods.Update(unfinished);
                _context.SaveChanges();

                return true;
            }

            return false;
        }
    }
}