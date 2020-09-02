using Microsoft.AspNetCore.Hosting;

[assembly: HostingStartup(typeof(WorkTimer.Blazor.Areas.Identity.IdentityHostingStartup))]
namespace WorkTimer.Blazor.Areas.Identity {
    public class IdentityHostingStartup : IHostingStartup {
        public void Configure(IWebHostBuilder builder) {
            builder.ConfigureServices((context, services) => {
            });
        }
    }
}