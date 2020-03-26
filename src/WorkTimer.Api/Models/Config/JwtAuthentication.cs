using Microsoft.IdentityModel.Tokens;
using System;
using System.Text;

namespace WorkTimer.Api.Models.Config {
    public class JwtAuthentication {

        public string ServerSecret { get; set; }

        public string ValidAudience { get; set; }

        public string ValidIssuer { get; set; }

        public int TokenValidityMinutes { get; set; }

        public int TokenValidityHours { get; set; }

        public int TokenValidityDays { get; set; }

        private SigningCredentials _signingCredentials;
        public SigningCredentials SigningCredentials {
            get {
                _signingCredentials ??= GetSigningCredentials();
                return _signingCredentials;
            }
        }

        public TimeSpan ValidFor => new TimeSpan(TokenValidityDays, TokenValidityHours, TokenValidityMinutes, 0);

        public string JtGuid => Guid.NewGuid().ToString();

        private SigningCredentials GetSigningCredentials() {
            if (!string.IsNullOrWhiteSpace(ServerSecret)) {
                var secretBytes = Encoding.UTF8.GetBytes(ServerSecret);

                return new SigningCredentials(new SymmetricSecurityKey(secretBytes), SecurityAlgorithms.HmacSha512);
            }

            return default;
        }
    }
}
