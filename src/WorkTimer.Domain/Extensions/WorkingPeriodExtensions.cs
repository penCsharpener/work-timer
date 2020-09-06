using System;
using WorkTimer.Domain.Models;

namespace WorkTimer.Domain.Extensions {
    public static class WorkingPeriodExtensions {
        public static TimeSpan GetWorkTime(this WorkingPeriod period) {
            if (period.EndTime.HasValue) {
                return period.EndTime.Value - period.StartTime;
            }

            return DateTime.Now - period.StartTime;
        }
    }
}
