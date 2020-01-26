using Microsoft.Extensions.DependencyInjection;
using WorkTimer.Contracts;
using WorkTimer.Repositories;
using WorkTimer.Services;

namespace WorkTimer.Blazor.Extensions {
    public static class StartupConfigExtensions {
        public static IServiceCollection WireUpMockClasses(this IServiceCollection services) {
            services.AddSingleton<IWorkingDayRepository, MockWorkingDayRepository>();
            services.AddSingleton<IWorkPeriodRepository, MockWorkPeriodRepository>();
            services.AddSingleton<IWorkBreakRepository, MockWorkBreakRepository>();
            services.AddTransient<IToggleTracking, ToggleTrackingService>();
            services.AddTransient<IWriterWorkPeriod, MockWriterWorkPeriod>();
            return services;
        }
    }
}
