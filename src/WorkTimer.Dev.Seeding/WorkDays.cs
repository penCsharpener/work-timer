using System;
using System.Collections.Generic;
using System.Linq;
using WorkTimer.Domain.Models;

namespace WorkTimer.Dev.Seeding;
public static class WorkDays
{
    private static readonly Random _rnd = new();

    public static IEnumerable<WorkDay> GetEntities(int[] daysPast, int contractId)
    {
        foreach (var i in daysPast)
        {
            var date = DateTime.Now.AddDays(-i);

            yield return new WorkDay { ContractId = contractId, Date = date, WorkDayType = WorkDayType.Workday, WorkingPeriods = GetWorkingPeriods(date).ToList() };
        }
    }

    public static IEnumerable<WorkingPeriod> GetWorkingPeriods(DateTime date)
    {
        var periodCount = _rnd.Next(1, 3);

        for (var i = 1; i < periodCount + 1; i++)
        {
            (var start, var end) = GetStartEndTime(date, i, periodCount);

            yield return new WorkingPeriod { StartTime = start, EndTime = end };
        }
    }

    private static (DateTime start, DateTime end) GetStartEndTime(DateTime date, int current, int total)
    {
        return total == 1
            ? ((DateTime start, DateTime end)) (date.Date.AddSeconds(_rnd.Next(6 * 60 * 60, 8 * 60 * 60)), date.Date.AddSeconds(_rnd.Next(15 * 60 * 60, 17 * 60 * 60)))
            : current == 1
            ? ((DateTime start, DateTime end)) (date.Date.AddSeconds(_rnd.Next(6 * 60 * 60, 8 * 60 * 60)), date.Date.AddSeconds(_rnd.Next(12 * 60 * 60, (int) (12.5 * 60 * 60))))
            : ((DateTime start, DateTime end)) (date.Date.AddSeconds(_rnd.Next((int) (12.5 * 60 * 60), 13 * 60 * 60)), date.Date.AddSeconds(_rnd.Next(15 * 60 * 60, 17 * 60 * 60)));
    }
}