using System;

namespace WorkTimer.Blazor.Extensions
{
    public static class BaseExtensions
    {
        public static string ToTimeString(this TimeSpan timeSpan)
        {
            int hours = (timeSpan.Days * 24) + timeSpan.Hours;

            var hourPart = $"{hours.ToString().Replace("-", "").PadLeft(2, '0')}";
            var minutePart = $"{timeSpan.Minutes.ToString().Replace("-", "").PadLeft(2, '0')}";
            var secondPart = $"{timeSpan.Seconds.ToString().Replace("-", "").PadLeft(2, '0')}";
            var timeString = $"{hourPart}:{minutePart}:{secondPart}";

            if (timeSpan < TimeSpan.Zero)
            {
                return "-" + timeString;
            }

            return timeString;
        }
    }
}