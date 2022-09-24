using System;
using System.Collections.Generic;
using System.Globalization;
using WorkTimer.Domain.Models;

namespace WorkTimer.Domain.Extensions;

public static class DateTimeExtensions
{
    private static readonly Calendar _calendar = new CultureInfo("de-DE").Calendar;

    public static IEnumerable<DateTime> GetWholeWeek(this DateTime dateTime)
    {
        var weekStart = dateTime;

        while (weekStart.DayOfWeek != DayOfWeek.Monday)
        {
            weekStart = weekStart.AddDays(-1);
        }

        var list = new List<DateTime>();

        for (var i = 0; i < 7; i++)
        {
            list.Add(weekStart.AddDays(i));
        }

        return list;
    }

    public static int GetWeekNumber(this DateTime dateTime)
    {
        return _calendar.GetWeekOfYear(dateTime, CalendarWeekRule.FirstFullWeek, DayOfWeek.Monday);
    }

    public static int GetWeekNumber(int year, int calendarWeek, out DateTime dayInThatWeek)
    {
        dayInThatWeek = new DateTime(year, 01, 01);

        while (calendarWeek != _calendar.GetWeekOfYear(dayInThatWeek, CalendarWeekRule.FirstFullWeek, DayOfWeek.Monday) || dayInThatWeek.Year >= (year + 2))
        {
            dayInThatWeek = dayInThatWeek.AddDays(7);
        }

        return _calendar.GetWeekOfYear(dayInThatWeek, CalendarWeekRule.FirstFullWeek, DayOfWeek.Monday);
    }

    public static double GetRequiredHoursForMonth(Contract contract, int year, int month)
    {
        var date = new DateTime(year, month, 1);
        var result = 0d;
        var hoursPerDay = contract.HoursPerWeek / 5d;

        while (date.Month == month)
        {
            result += date.DayOfWeek is DayOfWeek.Sunday or DayOfWeek.Saturday ? 0 : hoursPerDay;

            date = date.AddDays(1);
        }

        return Math.Round(result, 2);
    }

    public static int GetTotalDaysInMonth(int month)
    {
        return month switch
        {
            1 => 31,
            2 => 28,
            3 => 31,
            4 => 30,
            5 => 31,
            6 => 30,
            7 => 31,
            8 => 31,
            9 => 30,
            10 => 31,
            11 => 30,
            12 => 31,
            _ => 0
        };
    }
}
