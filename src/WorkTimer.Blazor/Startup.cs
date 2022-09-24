using EasyNetQ;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MudBlazor.Services;
using NETCore.MailKit.Extensions;
using NETCore.MailKit.Infrastructure.Internal;
using WorkTimer.Blazor.Areas.Identity;
using WorkTimer.Blazor.Extensions;
using WorkTimer.Blazor.Middleware;
using WorkTimer.Blazor.Services;
using WorkTimer.Domain.Models;
using WorkTimer.Persistence.Extensions;

namespace WorkTimer.Blazor;

public class Startup
{
    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    public IConfiguration Configuration { get; }

    public void ConfigureServices(IServiceCollection services)
    {
        var mailSettings = Configuration.GetSection("MailKitOptions").Get<MailKitOptions>();
        var passwordOptions = Configuration.GetSection(nameof(PasswordOptions)).Get<PasswordOptions>();
        services.AddEntityFramework(Configuration);
        services.AddHttpContextAccessor();
        services.AddRazorPages();
        services.AddSingleton(_ => RabbitHutch.CreateBus(Configuration.GetConnectionString("RabbitMqConnection")));
        services.AddServerSideBlazor();
        services.AddWorkTimerServices();
        services.AddDatabaseDeveloperPageExceptionFilter();
        services.AddScoped<AuthenticationStateProvider, RevalidatingIdentityAuthenticationStateProvider<AppUser>>();
        services.AddScoped<TokenProvider>();
        services.AddSingleton(mailSettings);
        services.AddSingleton(passwordOptions);
        services.AddMailKit(options => options.UseMailKit(mailSettings));
        services.AddMudServices();
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }
        else
        {
            app.UseExceptionHandler("/Error");
            app.UseHsts();
        }

        app.UseForwardedHeaders(new ForwardedHeadersOptions { ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto });

        if (Configuration?.GetValue<string>("ApplicationSettings:LaunchUrls")?.Contains("https") == true)
        {
            app.UseHttpsRedirection();
        }

        app.UseMiddleware<ExceptionMiddleware>();
        app.UseStaticFiles();

        app.UseRouting();

        app.UseAuthentication();
        app.UseAuthorization();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
            endpoints.MapBlazorHub();
            endpoints.MapFallbackToPage("/_Host");
        });
    }
}