using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using WorkTimer.Api.Extensions;
using WorkTimer.Api.Models.Config;

namespace WorkTimer.Api {
    public sealed class Program {

        public static IConfiguration LaunchSettings { get; private set; }

        public static async Task Main(string[] args) {
            LaunchSettings = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", false, true)
                .AddUserSecrets<Program>(true, true)
                .Build();

            var host = CreateHostBuilder(args).Build();

            using (var scope = host.Services.CreateScope()) {
                var sp = scope.ServiceProvider;
                try {
                    await sp.MigrateDatabase();
                } catch (System.Exception ex) {
                    var logger = sp.GetRequiredService<ILogger<Program>>();
                    logger.LogError(ex, "An error occurred while migrating the database.");
                }
            }

            host.Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) {
            return Host.CreateDefaultBuilder(args)
                       .ConfigureWebHostDefaults(webBuilder => {
                           webBuilder.UseStartup<Startup>()
                           .UseKestrel()
                           .UseUrls(LaunchSettings.GetSection(nameof(ApplicationSettings)).Get<ApplicationSettings>().LaunchUrls);
                       });
        }
    }
}
