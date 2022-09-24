using System;
using System.Collections.Generic;
using WorkTimer.Domain.Models;

namespace WorkTimer.Dev.Seeding;

public class WorkingPeriodSeeder : Seeder<WorkingPeriod>
{
    private readonly int _workDayId;
    private readonly DateTime _date;
    private DateTime _lastTime;

    public WorkingPeriodSeeder(int workDayId, DateTime date, int countPeriods)
    {
        _workDayId = workDayId;
        _date = date.Date;

        if (countPeriods > 0)
        {
            for (var i = 1; i <= countPeriods; i++)
            {
                AddPeriod(_random.Next(10, 50), _random.Next(2 * 60, (4 * 60) + 20));
            }
        }
    }

    public WorkingPeriodSeeder AddPeriod(double minutesBreak, double minutesPeriodLength)
    {
        var wp = new WorkingPeriod
        {
            WorkDayId = _workDayId,
            StartTime = _list.Count == 0 ? GetWorkDayStart() : AddBreak(minutesBreak),

            EndTime = GetPeriodLength(minutesPeriodLength)
        };

        _list.Add(wp);

        return this;
    }

    public override IList<WorkingPeriod> Seed()
    {
        return _list;
    }

    private DateTime GetWorkDayStart()
    {
        _lastTime = _date.AddMinutes(_random.Next(6 * 60, 9 * 60));

        return _lastTime;
    }

    private DateTime AddBreak(double minutesBreak)
    {
        _lastTime = _lastTime.AddMinutes(minutesBreak);

        return _lastTime;
    }

    private DateTime GetPeriodLength(double minutesPeriodLength)
    {
        _lastTime = _lastTime.AddMinutes(minutesPeriodLength);

        return _lastTime;
    }
}
