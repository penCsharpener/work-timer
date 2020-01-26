using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace WorkTimer.Blazor {
    public sealed class Program {
        public static void Main(string[] args) {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) {
            return Host.CreateDefaultBuilder(args)
                       .ConfigureAppConfiguration(conf => {
                           //conf.SetBasePath(Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location))
                           //.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                           //.AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")}.json", optional: false, reloadOnChange: true)
                           //.AddUserSecrets<Program>(optional: true, reloadOnChange: true)
                           //.AddCommandLine(args);
                       })
                       .ConfigureWebHostDefaults(webBuilder => {
                           webBuilder.UseStartup<Startup>();
                       });
        }
    }
}
