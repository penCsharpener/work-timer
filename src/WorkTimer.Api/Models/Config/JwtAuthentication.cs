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

        public SymmetricSecurityKey SymmetricSecurityKey {
            get {
                if (string.IsNullOrWhiteSpace(ServerSecret)) {
                    throw new ArgumentNullException(nameof(ServerSecret));
                }

                var secretBytes = Encoding.UTF8.GetBytes(ServerSecret);
                return new SymmetricSecurityKey(secretBytes);
            }
        }

        public TimeSpan ValidFor => new TimeSpan(TokenValidityDays, TokenValidityHours, TokenValidityMinutes, 0);

        public string JtGuid => Guid.NewGuid().ToString();

        private SigningCredentials GetSigningCredentials() {
            if (!string.IsNullOrWhiteSpace(ServerSecret)) {


                return new SigningCredentials(SymmetricSecurityKey, SecurityAlgorithms.HmacSha512);
            }

            return default;
        }
    }
}
