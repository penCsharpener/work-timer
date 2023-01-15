using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Threading.Tasks;
using WorkTimer.Domain.Models;
using WorkTimer.Persistence.Data;

namespace WorkTimer.Persistence.Extensions;
public static class DatabaseExtensions
{
    public static async Task MigrateDatabaseAsync(this IServiceProvider sp)
    {
        await sp.GetRequiredService<AppDbContext>().Database.MigrateAsync();
    }

    public static void AddEntityFramework(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<AppDbContext>(options => options.UseSqlite(configuration.GetConnectionString("DefaultConnection")), ServiceLifetime.Transient);

        services.AddDefaultIdentity<AppUser>(options =>
        {
            options.SignIn.RequireConfirmedAccount = true;
            options.Password = configuration.GetSection(nameof(PasswordOptions)).Get<PasswordOptions>()!;
        }).AddRoles<AppRole>().AddEntityFrameworkStores<AppDbContext>();
    }

    public static async Task SeedDatabase(this IServiceProvider sp)
    {
        using var scope = sp.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();

        var todoCount = await context.Todos.CountAsync();

        if (todoCount == 0)
        {
            var todos = Enumerable.Range(1, 200).Select(i => new Todo()
            {
                Id = i,
                ContractId = i % 23 == 0 ? 2 : 1,
                CreatedOn = DateTime.Now.AddDays(-i + 1),
                Deadline = i % 17 == 0 ? DateTime.Now.AddDays(i) : null,
                Description = $"Description " + i,
                IsContractIndependent = i % 47 == 0,
                Priority = i % 19 == 0 ? Domain.Models.Enums.TodoPriority.High : Domain.Models.Enums.TodoPriority.Medium,
                Title = "Title " + i,
                UserId = 1,
            }).OrderBy(t => t.Deadline ?? DateTime.MaxValue)
            .ThenByDescending(t => t.Priority)
            .ThenBy(t => t.CreatedOn)
            .ToList();

            context.Todos.AddRange(todos);

            await context.SaveChangesAsync();
        }
    }
}