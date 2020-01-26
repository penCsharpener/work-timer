using System;
using System.IO;
using WorkTimer.Config;

namespace WorkTimer {
    public static class Extensions {
        public static string ToConnectionString(this SqliteConfiguration config) {
            return $"Data Source={Path.Combine(Environment.ExpandEnvironmentVariables(config.PathToDatabase), config.DatabaseFileName)};Version=3;" +
                $"Compress=True;UTF8Encoding=True;";
        }

        public static string ToTimeString(this TimeSpan timeSpan) {
            return $"{timeSpan.Hours.ToString().PadLeft(2, '0')}:{timeSpan.Minutes.ToString().PadLeft(2, '0')}:{timeSpan.Seconds.ToString().PadLeft(2, '0')}";
        }
    }
}
