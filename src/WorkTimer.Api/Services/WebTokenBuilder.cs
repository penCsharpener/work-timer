using Microsoft.Extensions.Options;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using WorkTimer.Api.Contracts;
using WorkTimer.Api.Extensions;
using WorkTimer.Api.Models;
using WorkTimer.Api.Models.Config;

namespace WorkTimer.Api.Services {
    public class WebTokenBuilder : IWebTokenBuilder {
        private readonly IOptions<JwtAuthentication> _jwtOptions;

        public WebTokenBuilder(IOptions<JwtAuthentication> jwtOptions) {
            _jwtOptions = jwtOptions;
        }

        public string GenerateToken(User user) {
            if (_jwtOptions.Value == null) {
                throw new ArgumentNullException(nameof(_jwtOptions.Value));
            }

            if (_jwtOptions.Value.ServerSecret == null) {
                throw new ArgumentNullException(nameof(_jwtOptions.Value.ServerSecret));
            }

            if (_jwtOptions.Value.ValidFor <= TimeSpan.Zero) {
                throw new ArgumentException("Must be a non-zero TimeSpan.", nameof(_jwtOptions.Value.ValidFor));
            }

            var now = DateTime.Now;

            var claims = new[] {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim(JwtRegisteredClaimNames.Jti, _jwtOptions.Value.JtGuid),
                new Claim(JwtRegisteredClaimNames.Iat, now.ToUnixEpochDate().ToString(), ClaimValueTypes.Integer64),
            };

            var token = new JwtSecurityToken(
                issuer: _jwtOptions.Value.ValidIssuer,
                audience: _jwtOptions.Value.ValidAudience,
                claims: claims,
                notBefore: now,
                expires: now.Add(_jwtOptions.Value.ValidFor),
                signingCredentials: _jwtOptions.Value.SigningCredentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
