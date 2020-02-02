using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace WorkTimer.Api {
    public sealed class Program {
        public static void Main(string[] args) {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) {
            return Host.CreateDefaultBuilder(args)
                       .ConfigureWebHostDefaults(webBuilder => {
                           webBuilder.UseStartup<Startup>()
                           .UseKestrel()
                           .UseUrls("https://localhost:5661;http://localhost:5660");
                       });
        }
    }
}
