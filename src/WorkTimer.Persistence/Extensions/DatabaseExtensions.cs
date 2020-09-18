using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;
using WorkTimer.Domain.Models;
using WorkTimer.Persistence.Data;

namespace WorkTimer.Persistence.Extensions {
    public static class DatabaseExtensions {
        public static async Task MigrateDatabaseAsync(this IServiceProvider sp) {
            await sp.GetService<AppDbContext>().Database.MigrateAsync();
        }

        public static void AddEntityFramework(this IServiceCollection services, IConfiguration configuration) {
            services.AddDbContext<AppDbContext>(options => options.UseSqlite(configuration.GetConnectionString("DefaultConnection")), ServiceLifetime.Transient);
            services.AddDefaultIdentity<AppUser>(options => {
                options.SignIn.RequireConfirmedAccount = true;
                options.Password = configuration.GetSection(nameof(PasswordOptions)).Get<PasswordOptions>();
            }).AddRoles<AppRole>().AddEntityFrameworkStores<AppDbContext>();
        }
    }
}
