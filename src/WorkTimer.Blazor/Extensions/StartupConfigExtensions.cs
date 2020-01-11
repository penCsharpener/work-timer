using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WorkTimer.Contracts;
using WorkTimer.Repositories;

namespace WorkTimer.Blazor.Extensions {
    public static class StartupConfigExtensions {
        public static IServiceCollection WireUpMockClasses(this IServiceCollection services) {
            services.AddSingleton<IWorkingDayRepository, MockWorkingDayRepository>();
            services.AddSingleton<IWorkPeriodRepository, MockWorkPeriodRepository>();
            return services;
        }
    }
}
