using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Data.SQLite;
using WorkTimer.Config;
using WorkTimer.Contracts;
using WorkTimer.EF;
using WorkTimer.Models;
using WorkTimer.Repositories;
using WorkTimer.Services;

namespace WorkTimer.Blazor {
    public class Startup {
        public Startup(IConfiguration configuration) {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services) {
            services.AddRazorPages();
            services.AddDbContextPool<AppDbContext>(options => {
                options.UseSqlite(Environment.ExpandEnvironmentVariables(Configuration.GetConnectionString("SqliteEFCoreIdentity")), opt => {
                    opt.MigrationsAssembly("WorkTimer.EF");
                });
            });
            //services.AddEntityFrameworkSqlite();
            services.AddIdentity<AppUser, IdentityRole<int>>().AddEntityFrameworkStores<AppDbContext>();
            services.AddServerSideBlazor();
            services.Configure<SqliteConfiguration>(Configuration.GetSection(nameof(SqliteConfiguration)));
            services.AddScoped<IWorkPeriodWriter, WorkPeriodWriter>();
            services.AddScoped<IWorkPeriodRepository, WorkPeriodRepository>();
            services.AddScoped<IToggleTracking, ToggleTrackingService>();
            services.AddScoped<IWriterWorkPeriod, WriterWorkPeriod>();
            services.AddScoped<IWorkingDayRepository, WorkingDayRepository>();
            services.AddScoped<IWorkBreakRepository, MockWorkBreakRepository>();
            services.AddScoped<IDbInitService, DbInitService>();
            services.AddSingleton<IDatabaseConnection<SQLiteConnection>, SqliteDatabaseConnectionService>();
            //#if DEBUG
            //            services.WireUpMockClasses();
            //#else
            //            services.AddScoped<IWorkingDayRepository, WorkingDayRepository>();
            //#endif
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env) {
            if (env.IsDevelopment()) {
                app.UseDeveloperExceptionPage();
            } else {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseAuthentication();

            app.UseRouting();

            app.UseEndpoints(endpoints => {
                endpoints.MapBlazorHub();
                endpoints.MapFallbackToPage("/_Host");
            });
            
        }
    }
}
