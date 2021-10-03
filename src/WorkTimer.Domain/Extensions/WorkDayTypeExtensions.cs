using Nager.Date;
using System;
using WorkTimer.Domain.Models;

namespace WorkTimer.Domain.Extensions
{
    public static class WorkDayTypeExtensions
    {
        public static WorkDayType ToWorkDayType(this DateTime date)
        {
            if (DateSystem.IsPublicHoliday(date, CountryCode.DE, "DE-SN"))
            {
                return WorkDayType.BankHoliday;
            }

            if (DateSystem.IsWeekend(date, CountryCode.DE))
            {
                return WorkDayType.Weekend;
            }

            return WorkDayType.Workday;
        }
    }
}
