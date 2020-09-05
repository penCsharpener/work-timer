using System;
using System.Collections.Generic;
using System.Linq;
using WorkTimer.Domain.Models;

namespace WorkTimer.Dev.Seeding {
    public static class WorkDays {

        public static IEnumerable<WorkDay> GetEntities(int[] daysPast, int contractId) {
            foreach (var i in daysPast) {
                var date = DateTime.Now.AddDays(-i);
                yield return new WorkDay { ContractId = contractId, Date = date, WorkDayType = WorkDayType.Workday, WorkingPeriods = GetWorkingPeriods(date).ToList() };
            }
        }

        public static IEnumerable<WorkingPeriod> GetWorkingPeriods(DateTime date) {
            var startEnd = GetStartEndTime(date);
            yield return new WorkingPeriod { StartTime = startEnd.start, EndTime = startEnd.end };
        }

        private static Random _rnd = new Random();

        private static (DateTime start, DateTime end) GetStartEndTime(DateTime date) {
            var startTime = _rnd.Next(6 * 60 * 60, 8 * 60 * 60);
            var endTime = _rnd.Next(15 * 60 * 60, 17 * 60 * 60);
            return (date.Date.AddSeconds(startTime), date.Date.AddSeconds(endTime));
        }
    }
}
