using EasyNetQ;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using WorkTimer.Domain.Extensions;
using WorkTimer.Domain.Models;
using WorkTimer.Messaging.MessageModels;
using WorkTimer.Persistence.Data;

namespace WorkTimer.Messaging
{
    public class MessageWorker : BackgroundService
    {
        private readonly IBus _bus;
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<MessageWorker> _logger;

        public MessageWorker(IBus bus, IServiceProvider serviceProvider, ILogger<MessageWorker> logger)
        {
            _bus = bus;
            _serviceProvider = serviceProvider;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await _bus.PubSub.SubscribeAsync<RecalculateStatsMessage>("1", RecalculateHoursOfUserAsync, (subConfig) => { }, stoppingToken);
            await _bus.PubSub.SubscribeAsync<UpdateTotalHoursFromWorkDayMessage>("2", UpdateTotalHoursFromWorkDayAsync, (subConfig) => { }, stoppingToken);
            await _bus.PubSub.SubscribeAsync<UpdateOnEditWorkdayMessage>("3", UpdateOnEditWorkdayAsync, (subConfig) => { }, stoppingToken);
        }

        internal async Task RecalculateHoursOfUserAsync(RecalculateStatsMessage message, CancellationToken stoppingToken)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                using (var context = scope.ServiceProvider.GetRequiredService<AppDbContext>())
                {
                    List<WorkDay> workdays = await context.WorkDays.Include(x => x.Contract).Include(x => x.WorkingPeriods).ToListAsync(stoppingToken);

                    foreach (WorkDay workDay in workdays)
                    {
                        UpdateTotalHoursOfWorkDay(workDay);
                        workDay.RequiredHours = workDay.GetRequiredHoursForDay(workDay.Contract.HoursPerWeek);
                    }

                    await UpdateTotalOverhoursOfContract(context, workdays[0].Contract);

                    await context.SaveChangesAsync();

                    _logger.LogInformation("Processed {workdaysCount} work days via queue.", workdays.Count);
                }
            }
        }

        internal async Task UpdateTotalHoursFromWorkDayAsync(UpdateTotalHoursFromWorkDayMessage message, CancellationToken cancellationToken)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                using (var context = scope.ServiceProvider.GetRequiredService<AppDbContext>())
                {
                    var workDay = await context.WorkDays.Include(x => x.WorkingPeriods).Include(wd => wd.Contract).FirstOrDefaultAsync(x => x.Id == message.WorkdayId, cancellationToken);

                    if (workDay == null)
                    {
                        _logger.LogInformation("No work day found for id {WorkdayId}", message.WorkdayId);
                        return;
                    }

                    if (workDay.WorkingPeriods.Count == 0)
                    {
                        context.WorkDays.Remove(workDay);

                        await context.SaveChangesAsync();

                        _logger.LogInformation("Workday of {workDayDate} didn't have any work periods and was deleted.", workDay.Date);

                        return;
                    }

                    workDay.TotalHours = CalculateTotalHoursFromWorkDay(workDay);

                    await UpdateTotalOverhoursOfContract(context, workDay.Contract);

                    await context.SaveChangesAsync();

                    _logger.LogInformation("Processed {workDayDate} via queue.", workDay.Date);
                }
            }
        }

        internal async Task UpdateOnEditWorkdayAsync(UpdateOnEditWorkdayMessage message, CancellationToken cancellationToken)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                using (var context = scope.ServiceProvider.GetRequiredService<AppDbContext>())
                {
                    var workDay = await context.WorkDays.Include(wd => wd.Contract).Include(x => x.WorkingPeriods).FirstOrDefaultAsync(x => x.WorkingPeriods.Any(wp => wp.WorkDayId == message.WorkdayId), cancellationToken);
                    CorrectWorkDayDateBasedOnPeriods(workDay);
                    UpdateTotalHoursOfWorkDay(workDay);

                    await UpdateTotalOverhoursOfContract(context, workDay.Contract);

                    await context.SaveChangesAsync();

                    _logger.LogInformation("Processed workDay {workDayDate} via queue.", workDay.Date);
                }
            }
        }

        private async Task UpdateTotalHoursOfWorkDayAsync(AppDbContext context, int workdayId)
        {
            var workday = await context.WorkDays.Include(x => x.WorkingPeriods)
                .Where(x => x.Id == workdayId && x.WorkingPeriods.Any(wp => wp.EndTime.HasValue))
                .Select(x => x)
                .FirstOrDefaultAsync();

            UpdateTotalHoursOfWorkDay(workday);
        }

        private void UpdateTotalHoursOfWorkDay(WorkDay workday)
        {
            if (workday?.WorkingPeriods.Count > 0)
            {
                double totalHours = workday.WorkingPeriods.Where(x => x.EndTime.HasValue).Sum(x => (x.EndTime.Value - x.StartTime).TotalHours);
                workday.TotalHours = totalHours;
            }
        }

        private double CalculateTotalHoursFromWorkDay(WorkDay workDay)
        {
            if (workDay.WorkingPeriods.Count == 0)
            {
                return 0;
            }

            workDay.TotalHours = workDay.WorkingPeriods.Where(x => x.EndTime.HasValue).Sum(x => (x.EndTime.Value - x.StartTime).TotalHours);

            return workDay.TotalHours;
        }

        private void CorrectWorkDayDateBasedOnPeriods(WorkDay workday)
        {
            var workingPeriods = workday.WorkingPeriods.Select(x => new { x.StartTime.Date }).ToList();

            if (workingPeriods?.Count > 0)
            {
                int count = workingPeriods.GroupBy(x => x.Date).Count();

                if (count == 1)
                {
                    DateTime date = workingPeriods.FirstOrDefault().Date;

                    if (date != workday.Date)
                    {
                        workday.Date = date;
                    }
                }
            }
        }

        // TODO: instead of passing userId, pass contract of workday and update hours there directly
        private async Task UpdateTotalOverhoursOfContract(AppDbContext context, Contract currentContract)
        {
            var allHours = await context.WorkingPeriods.Include(x => x.WorkDay).ThenInclude(x => x.Contract)
                .Where(x => x.WorkDay.Contract.UserId == currentContract.UserId && x.WorkDay.Contract.IsCurrent && x.WorkDay.WorkingPeriods.All(x => x.EndTime.HasValue))
                .SumAsync(x => x.WorkDay.TotalHours - x.WorkDay.RequiredHours);

            var overHoursCompensated = await context.OverHoursCompensations.Where(c => c.Contract.IsCurrent && c.Contract.UserId == currentContract.UserId).SumAsync(oh => oh.OverHours);

            allHours -= overHoursCompensated;

            currentContract.TotalOverhours = TimeSpan.FromHours(allHours);
        }

        internal record TotalHoursCalculationModel(double TotalHours, double RequiredHours);
    }
}
