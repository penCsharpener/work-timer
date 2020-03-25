using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;
using WorkTimer.EF;
using WorkTimer.EF.Models;

namespace WorkTimer.Api.Extensions {
    public static class ServiceExtensions {
        public static void AddIdentity(this IServiceCollection services, PasswordOptions options) {
            services.AddIdentity<AppUser, AppRole>(setup => setup.Password = options)
                    .AddEntityFrameworkStores<AppDbContext>()
                    .AddDefaultTokenProviders();
        }

        public static async Task MigrateDatabase(this IServiceProvider sp) {
            await sp.GetService<AppDbContext>().Database.MigrateAsync();
        }
    }
}
