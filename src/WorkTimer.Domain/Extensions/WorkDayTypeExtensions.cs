using Nager.Date;
using System;
using WorkTimer.Domain.Models;
using static Nager.Date.DateSystem;

namespace WorkTimer.Domain.Extensions
{
    public static class WorkDayTypeExtensions
    {
        public static WorkDayType ToWorkDayType(this DateTime date)
        {
            if (IsPublicHoliday(date, CountryCode.DE, "DE-SN"))
            {
                return WorkDayType.BankHoliday;
            }

            if (IsWeekend(date, CountryCode.DE))
            {
                return WorkDayType.Weekend;
            }

            return WorkDayType.Workday;
        }
    }
}
