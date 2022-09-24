using System;
using System.Linq;
using WorkTimer.Domain.Models;

namespace WorkTimer.Domain.Extensions;

public static class WorkDayExtensions
{
    public static bool IsUnfinishedWorkday(this WorkDay workday)
    {
        return workday.WorkingPeriods.Any(wp => !wp.EndTime.HasValue);
    }

    public static WorkingPeriod? GetOngoingWorkingPeriod(this WorkDay workDay)
    {
        return workDay.WorkingPeriods.FirstOrDefault(wp => wp.EndTime is null);
    }

    public static double GetContractedHoursPerDay(this Contract contract)
    {
        var contractedHours = contract?.HoursPerWeek ?? 0;

        return contractedHours / 5d;
    }

    public static double GetRequiredHoursForDay(this WorkDay workDay, int hoursPerWeek)
    {
        var dailyHours = hoursPerWeek / 5d;

        return dailyHours * workDay.GetWorkHourMultiplier();
    }

    public static double GetOverhours(this WorkDay workDay)
    {
        return workDay.TotalHours - workDay.RequiredHours;
    }

    public static TimeSpan GetWorkTime(this WorkDay workDay)
    {
        return TimeSpan.FromHours(workDay.TotalHours);
    }

    public static double GetWorkHourMultiplier(this WorkDay workDay)
    {
        return workDay.WorkDayType.GetWorkHourMultiplier();
    }

    public static double GetWorkHourMultiplier(this WorkDayType workDayType)
    {
        return workDayType switch
        {
            WorkDayType.HalfVacation => 0.5,
            WorkDayType.Workday => 1,
            WorkDayType.Undefined => 1,
            _ => 0
        };
    }

    public static WorkDay CalculateTotalHours(this WorkDay workday)
    {
        if (workday.WorkingPeriods?.Count > 0)
        {
            workday.TotalHours = workday.WorkingPeriods.Where(x => x.EndTime.HasValue).Sum(x => (x.EndTime.Value - x.StartTime).TotalHours);
        }

        return workday;
    }
}