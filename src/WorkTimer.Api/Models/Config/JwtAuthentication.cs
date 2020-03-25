﻿namespace WorkTimer.Api.Models.Config {
    public class JwtAuthentication {
        public string ServerSecret { get; set; }
        public string ValidAudience { get; set; }
        public string ValidIssuer { get; set; }
    }
}
