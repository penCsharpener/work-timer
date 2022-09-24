using Nager.Date;
using System;
using WorkTimer.Domain.Models;

namespace WorkTimer.Domain.Extensions;

public static class WorkDayTypeExtensions
{
    static WorkDayTypeExtensions()
    {
        DateSystem.LicenseKey = "LostTimeIsNeverFoundAgain";
    }

    public static WorkDayType ToWorkDayType(this DateTime date)
    {
        return DateSystem.IsPublicHoliday(date, CountryCode.DE, "DE-SN")
            ? WorkDayType.BankHoliday
            : DateSystem.IsWeekend(date, CountryCode.DE) ? WorkDayType.Weekend : WorkDayType.Workday;
    }
}
