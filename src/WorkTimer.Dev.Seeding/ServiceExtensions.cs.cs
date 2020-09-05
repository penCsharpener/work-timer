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

            if (!await context.SeedUsers(userManager)) {
                context.Contracts.AddRange(Contracts.GetEntities());
                context.SaveChanges();
                context.WorkDays.AddRange(WorkDays.GetEntities(new[] { 1, 2, 3, 4, 5, 6, 7, 8, 9 }, 1));
            }

            await context.SaveChangesAsync();
        }
    }
}
