using System;
using WorkTimer.Domain.Models;

namespace WorkTimer.Domain.Extensions;

public static class WorkingPeriodExtensions
{
    public static TimeSpan GetWorkTime(this WorkingPeriod period)
    {
        return period.EndTime.HasValue ? period.EndTime.Value - period.StartTime : DateTime.Now - period.StartTime;
    }
}