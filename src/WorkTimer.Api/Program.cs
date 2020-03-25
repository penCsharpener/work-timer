using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using WorkTimer.Api.Models.Config;

namespace WorkTimer.Api {
    public sealed class Program {

        public static IConfiguration LaunchSettings { get; private set; }

        public static void Main(string[] args) {
            LaunchSettings = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", false, true)
                .AddUserSecrets<Program>(true, true)
                .Build();

            CreateHostBuilder(args).Build().Run();
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
