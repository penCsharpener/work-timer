using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using WorkTimer.Dev.Seeding;
using WorkTimer.Persistence.Extensions;

namespace WorkTimer.Blazor {
    public sealed class Program {
        public static IConfiguration LaunchSettings { get; private set; }

        public static async Task Main(string[] args) {
            try {
                LaunchSettings = new ConfigurationBuilder()
                    .AddJsonFile("appsettings.json", false, true)
                    .AddUserSecrets<Program>(true, true)
                    .Build();

                var host = CreateHostBuilder(args).Build();

                using (var scope = host.Services.CreateScope()) {
                    var sp = scope.ServiceProvider;
                    try {
                        await sp.MigrateDatabaseAsync();
                        await sp.SeedDatabaseAsync();
                    } catch (Exception ex) {
                        var logger = sp.GetRequiredService<ILogger<Program>>();
                        logger.LogError(ex, "An error occurred while migrating the database.");
                    }
                }

                host.Run();

            } catch {
                // no serilog yet
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args) {
            return Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder => {
                    webBuilder.UseStartup<Startup>()
                              .UseUrls("http://localhost:4660,https://localhost:4661");
                });
        }
    }
}
