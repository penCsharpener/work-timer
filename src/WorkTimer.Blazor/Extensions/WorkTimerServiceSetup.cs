using MediatR;
using Microsoft.Extensions.DependencyInjection;
using WorkTimer.MediatR.Models;
using WorkTimer.MediatR.Pipelines;
using WorkTimer.MediatR.Services;
using WorkTimer.MediatR.Services.Abstractions;

namespace WorkTimer.Blazor.Extensions
{
    public static class WorkTimerServiceSetup
    {
        public static void AddWorkTimerServices(this IServiceCollection services)
        {
            services.AddMediatR(typeof(UserContext).Assembly);
            services.AddSingleton<INow, NowTimeProvider>();
            services.AddScoped(typeof(IPipelineBehavior<,>), typeof(UserIdPipeline<,>));
        }
    }
}