using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WorkTimer.Contracts;
using WorkTimer.Repositories;
using WorkTimer.Services;

namespace WorkTimer.Blazor.Extensions {
    public static class StartupConfigExtensions {
        public static IServiceCollection WireUpMockClasses(this IServiceCollection services) {
            services.AddSingleton<IWorkingDayRepository, MockWorkingDayRepository>();
            services.AddSingleton<IWorkPeriodRepository, MockWorkPeriodRepository>();
            services.AddSingleton<IWorkBreakRepository, MockWorkBreakRepository>();
            services.AddTransient<IStartTracking, MockStartTracking>();
            services.AddTransient<IWriterWorkingDay, MockWriterWorkingDays>();
            services.AddTransient<IWriterWorkPeriod, MockWriterWorkPeriod>();
            services.AddTransient<IWriterWorkBreak, MockWriterWorkBreak>();
            return services;
        }
    }
}
