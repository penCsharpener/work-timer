using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using WorkTimer.EF;
using WorkTimer.EF.Models;

namespace WorkTimer.Api.Extensions {
    public static class ServiceCollectionExtensions {
        public static void AddIdentity(this IServiceCollection services, PasswordOptions options) {
            services.AddIdentity<AppUser, AppRole>(setup => setup.Password = options)
                    .AddEntityFrameworkStores<AppDbContext>()
                    .AddDefaultTokenProviders();
        }
    }
}
