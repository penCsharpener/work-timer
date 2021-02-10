using MediatR;
using Microsoft.Extensions.DependencyInjection;
using WorkTimer.MediatR.Models;
using WorkTimer.MediatR.Pipelines;

namespace WorkTimer.Blazor.Extensions {
    public static class WorkTimerServiceSetup {
        public static void AddWorkTimerServices(this IServiceCollection services) {
            services.AddMediatR(typeof(UserContext).Assembly);
            services.AddScoped(typeof(IPipelineBehavior<,>), typeof(UserIdPipeline<,>));
        }
    }
}