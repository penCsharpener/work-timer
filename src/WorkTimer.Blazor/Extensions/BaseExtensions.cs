﻿using System;

namespace WorkTimer.Blazor.Extensions {
    public static class BaseExtensions {
        public static string ToTimeString(this TimeSpan timeSpan) {
            int hours = (timeSpan.Days * 24) + timeSpan.Hours;

            string? s = $"{hours.ToString().Replace("-", "").PadLeft(2, '0')}" +
                        $":{timeSpan.Minutes.ToString().Replace("-", "").PadLeft(2, '0')}" +
                        $":{timeSpan.Seconds.ToString().Replace("-", "").PadLeft(2, '0')}";

            if (timeSpan < TimeSpan.Zero) {
                return "-" + s;
            }

            return s;
        }
    }
}