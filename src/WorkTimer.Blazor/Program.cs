using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;
using System;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;
using WorkTimer.Persistence.Extensions;

namespace WorkTimer.Blazor;

public sealed class Program
{
    public static IConfiguration? LaunchSettings { get; private set; }

    public static async Task Main(string[] args)
    {
        LaunchSettings = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json", false, true)
            .AddUserSecrets<Program>(true, true)
            .Build();

        Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(LaunchSettings)
                .CreateLogger();

        try
        {
            Log.Information("Starting up");

            Thread.CurrentThread.CurrentCulture = new CultureInfo("de-DE");
            Thread.CurrentThread.CurrentUICulture = new CultureInfo("de-DE");

            var host = CreateHostBuilder(args).Build();

            using (var scope = host.Services.CreateScope())
            {
                var sp = scope.ServiceProvider;

                try
                {
                    await sp.MigrateDatabaseAsync();

#if DEBUG
                    await sp.SeedDatabase();
#endif
                }
                catch (Exception ex)
                {
                    var logger = sp.GetRequiredService<ILogger<Program>>();
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
            .ConfigureWebHostDefaults(webBuilder => webBuilder.UseStartup<Startup>())
            .UseSerilog(Log.Logger);
    }
}