using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using WorkTimer.Config;

namespace WorkTimer {
    public static class Extensions {
        public static string ToConnectionString(this SqliteConfiguration config) {
            return $"Data Source={Path.Combine(Environment.ExpandEnvironmentVariables(config.PathToDatabase), config.DatabaseFileName)};Version=3;" +
                $"{(string.IsNullOrEmpty(config.DatabasePassword) ? "" : $"Password={config.DatabasePassword};")}" +
                $"Compress=True;UTF8Encoding=True;";
        }
    }
}
