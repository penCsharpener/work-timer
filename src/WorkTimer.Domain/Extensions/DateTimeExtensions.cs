using System;
using System.Collections.Generic;

namespace WorkTimer.Domain.Extensions
{
    public static class DateTimeExtensions
    {
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
    }
}
