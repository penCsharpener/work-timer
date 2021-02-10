using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;
using System;
using System.Threading.Tasks;
using WorkTimer.Dev.Seeding;
using WorkTimer.Persistence.Extensions;

namespace WorkTimer.Blazor
{
    public sealed class Program
    {
        public static IConfiguration? LaunchSettings { get; private set; }

        public static async Task Main(string[] args)
        {
            LaunchSettings = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", false, true)
                .AddUserSecrets<Program>(true, true)
                .Build();

            try
            {
                Log.Logger = new LoggerConfiguration()
                    .ReadFrom.Configuration(LaunchSettings)
                    .CreateLogger();

                Log.Information("Starting up");

                IHost? host = CreateHostBuilder(args).Build();

                using (IServiceScope? scope = host.Services.CreateScope())
                {
                    IServiceProvider? sp = scope.ServiceProvider;

                    try
                    {
                        await sp.MigrateDatabaseAsync();
                        await sp.SeedDatabaseAsync();
                    }
                    catch (Exception ex)
                    {
                        ILogger<Program>? logger = sp.GetRequiredService<ILogger<Program>>();
                        logger.LogError(ex, "An error occurred while migrating the database.");
                    }
                }

                await host.RunAsync();
            }
            catch (Exception e)
            {
                Log.Fatal(e, "Application start-up failed");
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args)
        {
            return Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>()
                        .UseUrls(LaunchSettings?.GetValue<string>("ApplicationSettings:LaunchUrls") ?? "https://localhost:4661;http://localhost:4660");
                }).UseSerilog(Log.Logger);
        }
    }
}