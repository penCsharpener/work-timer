using System;
using System.Collections.Generic;
using WorkTimer.Domain.Extensions;
using WorkTimer.Domain.Models;

namespace WorkTimer.Dev.Seeding;

public class WorkDaySeeder : Seeder<WorkDay>
{
    private readonly Contract _contract;

    public WorkDaySeeder(Contract contract)
    {
        _contract = contract;
    }

    public WorkDaySeeder AddWorkDayRange(string startDateString, string endDateString)
    {
        if (DateTime.TryParse(startDateString, out var startDate) && DateTime.TryParse(endDateString, out var endDate))
        {
            AddWorkDayRange(startDate, endDate);
        }

        return this;
    }

    public WorkDaySeeder AddWorkDayRange(DateTime startDate, DateTime endDate)
    {
        var date = startDate.Date;
        endDate = endDate.Date;

        while (date <= endDate)
        {
            AddWorkDay(date, GetPeriodCountRandomly());

            date = date.AddDays(1);
        }

        return this;
    }

    public WorkDaySeeder AddWorkDay(DateTime date, int numberOfPeriods)
    {
        if (date.ToWorkDayType() == WorkDayType.Workday)
        {
            var wd = new WorkDay { ContractId = _contract.Id, Date = date.Date, WorkingPeriods = new WorkingPeriodSeeder(0, date.Date, numberOfPeriods).Seed(), WorkDayType = date.ToWorkDayType() };
            wd.RequiredHours = wd.GetRequiredHoursForDay(_contract.HoursPerWeek);

            _list.Add(wd.CalculateTotalHours());
        }

        return this;
    }

    public override IList<WorkDay> Seed()
    {
        return _list;
    }

    private int GetPeriodCountRandomly()
    {
        var rnd = _random.Next(200, 300);

        return (int) Math.Round(rnd / 100d, 0);
    }
}
