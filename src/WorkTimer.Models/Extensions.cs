using System;
using System.Collections.Generic;
using System.IO;
using WorkTimer.Config;
using WorkTimer.Models;

namespace WorkTimer {
    public static class Extensions {
        public static string ToConnectionString(this SqliteConfiguration config) {
            return $"Data Source={Path.Combine(Environment.ExpandEnvironmentVariables(config.PathToDatabase), config.DatabaseFileName)};Version=3;" +
                $"Compress=True;UTF8Encoding=True;";
        }

        public static string ToTimeString(this TimeSpan timeSpan) {
            var s = $"{timeSpan.Hours.ToString().Replace("-", "").PadLeft(2, '0')}:{timeSpan.Minutes.ToString().Replace("-", "").PadLeft(2, '0')}:{timeSpan.Seconds.ToString().Replace("-", "").PadLeft(2, '0')}";
            if (timeSpan < TimeSpan.Zero) {
                return "-" + s;
            }
            return s;
        }

        public static WorkPeriodRaw ToRaw(this WorkPeriod wp) {
            return new WorkPeriodRaw() {
                Id = wp.Id,
                Comment = wp.Comment,
                StartTime = wp.StartTime.ToSqlite(),
                EndTime = wp.EndTime == null ? default(string?) : wp.EndTime.Value.ToSqlite(),
                IsBreak = wp.IsBreak ? 1 : 0,
            };
        }

        public static WorkPeriod FromRaw(this WorkPeriodRaw wpr) {
            return new WorkPeriod() {
                Id = wpr.Id,
                Comment = wpr.Comment,
                StartTime = DateTime.Parse(wpr.StartTime),
                EndTime = wpr.EndTime == null ? default(DateTime?) : DateTime.Parse(wpr.EndTime),
                IsBreak = Convert.ToBoolean(wpr.IsBreak),
                ExpectedHours = 8
            };
        }

        public static IEnumerable<WorkPeriodRaw> ToRaw(this IEnumerable<WorkPeriod> wps) {
            foreach (var item in wps) {
                yield return item.ToRaw();
            }
        }

        public static IEnumerable<WorkPeriod> FromRaw(this IEnumerable<WorkPeriodRaw> wprs) {
            foreach (var item in wprs) {
                yield return item.FromRaw();
            }
        }

        public static string ToSqlite(this DateTime dateTime) {
            return dateTime.ToString("yyyy-MM-dd HH:mm:ss");
        }
    }
}
