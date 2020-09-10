using System;
using System.Linq;
using WorkTimer.Domain.Models;

namespace WorkTimer.Domain.Extensions {
    public static class WorkDayExtensions {
        public static double GetContractedHoursPerDay(this WorkDay workDay) {
            var contractedHours = workDay.Contract?.HoursPerWeek ?? 0;

            return contractedHours / (double)5;
        }

        public static TimeSpan GetWorkTime(this WorkDay workDay) {
            if (workDay.WorkingPeriods?.Count == 0) {
                return new TimeSpan();
            }

            return TimeSpan.FromSeconds(workDay.WorkingPeriods.Sum(x => x.GetWorkTime().TotalSeconds));
        }

        public static double GetWorkHourMultiplier(this WorkDay workDay) {
            return workDay.WorkDayType.GetWorkHourMultiplier();
        }

        public static double GetWorkHourMultiplier(this WorkDayType workDayType) {
            return workDayType switch
            {
                WorkDayType.HalfVacation => 0.5,
                WorkDayType.Workday => 1,
                WorkDayType.Undefined => 1,
                _ => 0
            };
        }
    }
}
