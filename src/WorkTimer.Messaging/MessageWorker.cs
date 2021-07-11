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
        }

        private async Task RecalculateHoursOfUserAsync(RecalculateStatsMessage message, CancellationToken stoppingToken)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                using (var context = scope.ServiceProvider.GetRequiredService<AppDbContext>())
                {
                    List<WorkDay> workdays = await context.WorkDays.Include(x => x.Contract).Include(x => x.WorkingPeriods).ToListAsync();

                    foreach (WorkDay workDay in workdays)
                    {
                        UpdateTotalHoursOfWorkDay(workDay);
                        workDay.RequiredHours = workDay.GetRequiredHoursForDay(workDay.Contract.HoursPerWeek);
                    }

                    context.SaveChanges();

                    _logger.LogInformation($"Processed {workdays.Count} work days.");
                }
            }
        }

        private void UpdateTotalHoursOfWorkDay(AppDbContext context, int workdayId)
        {
            WorkDay? workday = context.WorkDays.Include(x => x.WorkingPeriods)
                .Where(x => x.Id == workdayId && x.WorkingPeriods.Any(wp => wp.EndTime.HasValue))
                .Select(x => x)
                .FirstOrDefault();

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
                return workDay.TotalHours;
            }

            workDay.TotalHours = workDay.WorkingPeriods.Where(x => x.EndTime.HasValue).Sum(x => (x.EndTime.Value - x.StartTime).TotalHours);

            return workDay.TotalHours;
        }
    }
}
