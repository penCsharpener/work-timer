using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Threading.Tasks;
using WorkTimer.Api.Models.Config;
using WorkTimer.EF;
using WorkTimer.EF.Models;

namespace WorkTimer.Api.Extensions {
    public static class ServiceExtensions {
        public static void AddIdentity(this IServiceCollection services, PasswordOptions options) {
            services.AddIdentity<AppUser, AppRole>(setup => setup.Password = options)
                    .AddEntityFrameworkStores<AppDbContext>()
                    .AddDefaultTokenProviders();
        }

        public static async Task MigrateDatabase(this IServiceProvider sp) {
            await sp.GetService<AppDbContext>().Database.MigrateAsync();
        }

        public static void AddJwtAuth(this IServiceCollection services, IConfiguration configuration) {
            var jwtOptions = configuration.GetSection(nameof(JwtAuthentication)).Get<JwtAuthentication>();

            services.AddAuthentication(opt => {
                opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(opt => {
                opt.ClaimsIssuer = jwtOptions.ValidIssuer;
                opt.TokenValidationParameters = GetTokenValidationParameters(jwtOptions);
                opt.SaveToken = true;
            });
        }

        private static TokenValidationParameters GetTokenValidationParameters(JwtAuthentication jwtOptions) {
            return new TokenValidationParameters {
                ValidateIssuer = true,
                ValidIssuer = jwtOptions.ValidIssuer,
                ValidateAudience = true,
                ValidAudience = jwtOptions.ValidAudience,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = jwtOptions.SymmetricSecurityKey,
                RequireExpirationTime = false,
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero
            };
        }
    }
}
