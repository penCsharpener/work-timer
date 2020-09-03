using MediatR;
using Microsoft.Extensions.DependencyInjection;
using WorkTimer.MediatR.Pipelines;

namespace WorkTimer.Blazor.Extensions {
    public static class WorkTimerServiceSetup {
        public static void AddWorkTimerServices(this IServiceCollection services) {
            services.AddMediatR(typeof(MediatR.Models.UserContext).Assembly);
            services.AddScoped(typeof(IPipelineBehavior<,>), typeof(UserIdPipeline<,>));
        }
    }
}
