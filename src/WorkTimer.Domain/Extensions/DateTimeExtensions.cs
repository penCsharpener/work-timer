using System;
using System.Collections.Generic;
using System.Globalization;

namespace WorkTimer.Domain.Extensions
{
    public static class DateTimeExtensions
    {
        private static Calendar _calendar = new CultureInfo("de-DE").Calendar;

        public static IEnumerable<DateTime> GetWholeWeek(this DateTime dateTime)
        {
            DateTime weekStart = dateTime;

            while (weekStart.DayOfWeek != DayOfWeek.Monday)
            {
                weekStart = weekStart.AddDays(-1);
            }

            var list = new List<DateTime>();

            for (int i = 0; i < 7; i++)
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
    }
}
