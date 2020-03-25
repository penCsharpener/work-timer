using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Threading.Tasks;
using WorkTimer.Api.Models.Config;

namespace WorkTimer.Api {
    public class Startup {
        public Startup(IConfiguration configuration) {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services) {
            services.Configure<JwtAuthentication>(Configuration.GetSection(nameof(JwtAuthentication)));
            services.AddAuthentication("OAuth")
                .AddJwtBearer("OAuth", config => {
                    var jwtOptions = Configuration.GetSection(nameof(JwtAuthentication)).Get<JwtAuthentication>();
                    var secretBytes = Encoding.UTF8.GetBytes(jwtOptions.ServerSecret);
                    var key = new SymmetricSecurityKey(secretBytes);

                    config.Events = new JwtBearerEvents() {
                        OnMessageReceived = context => {
                            if (context.Request.Query.ContainsKey("access_token")) {
                                context.Token = context.Request.Query["access_token"];
                            }

                            return Task.CompletedTask;
                        }
                    };

                    config.TokenValidationParameters = new TokenValidationParameters() {
                        ValidAudience = jwtOptions.ValidIssuer,
                        ValidIssuer = jwtOptions.ValidAudience,
                        IssuerSigningKey = key,
                    };
                });
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
