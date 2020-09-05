using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;
using WorkTimer.Domain.Models;
using WorkTimer.Persistence.Data;

namespace WorkTimer.Dev.Seeding {
    public static class ServiceExtensions {
        public static async Task SeedDatabaseAsync(this IServiceProvider provider) {
            var context = provider.GetRequiredService<AppDbContext>();
            var userManager = provider.GetRequiredService<UserManager<AppUser>>();

            await context.SeedUsers(userManager);

            await context.SaveChangesAsync();
        }
    }
}
