using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using WorkTimer.Api.Contracts;
using WorkTimer.Api.Extensions;
using WorkTimer.Api.Models.Config;
using WorkTimer.Api.Services;
using WorkTimer.EF;

namespace WorkTimer.Api {
    public class Startup {
        public Startup(IConfiguration configuration) {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services) {
            services.Configure<JwtAuthentication>(Configuration.GetSection(nameof(JwtAuthentication)));
            services.AddDbContext<AppDbContext>(options => options.UseSqlite(Configuration.GetConnectionString("DefaultConnection")));
            services.AddIdentity(Configuration.GetSection(nameof(PasswordOptions)).Get<PasswordOptions>());
            services.AddSingleton<IWebTokenBuilder, WebTokenBuilder>();
            services.AddScoped<IAuthProvider, AuthProvider>();
            services.AddJwtAuth(Configuration);
            services.AddControllers();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env) {
            if (env.IsDevelopment()) {
                app.UseDeveloperExceptionPage();
            }

            // app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints => {
                endpoints.MapControllers();
            });
        }
    }
}
