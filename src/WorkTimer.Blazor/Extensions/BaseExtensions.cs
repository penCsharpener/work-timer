using System;

namespace WorkTimer.Blazor.Extensions {
    public static class BaseExtensions {
        public static string ToTimeString(this TimeSpan timeSpan) {
            var s = $"{timeSpan.Hours.ToString().Replace("-", "").PadLeft(2, '0')}:{timeSpan.Minutes.ToString().Replace("-", "").PadLeft(2, '0')}:{timeSpan.Seconds.ToString().Replace("-", "").PadLeft(2, '0')}";
            if (timeSpan < TimeSpan.Zero) {
                return "-" + s;
            }
            return s;
        }

    }
}
