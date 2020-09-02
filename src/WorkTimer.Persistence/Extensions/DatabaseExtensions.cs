using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;
using WorkTimer.Blazor.Data;

namespace WorkTimer.Persistence.Extensions {
    public static class DatabaseExtensions {
        public static async Task MigrateDatabaseAsync(this IServiceProvider sp) {
            await sp.GetService<AppDbContext>().Database.MigrateAsync();
        }
    }
}
