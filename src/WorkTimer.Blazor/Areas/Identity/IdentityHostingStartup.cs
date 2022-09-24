using Microsoft.AspNetCore.Hosting;
using WorkTimer.Blazor.Areas.Identity;

[assembly: HostingStartup(typeof(IdentityHostingStartup))]

namespace WorkTimer.Blazor.Areas.Identity;
public class IdentityHostingStartup : IHostingStartup
{
    public void Configure(IWebHostBuilder builder)
    {
        builder.ConfigureServices((context, services) => { });
    }
}