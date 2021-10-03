using System;
using System.Collections.Generic;
using System.Linq;
using WorkTimer.Domain.Models;

namespace WorkTimer.Dev.Seeding {
    public static class WorkDays {
        private static readonly Random _rnd = new Random();

        public static IEnumerable<WorkDay> GetEntities(int[] daysPast, int contractId) {
            foreach (int i in daysPast) {
                DateTime date = DateTime.Now.AddDays(-i);

                yield return new WorkDay { ContractId = contractId, Date = date, WorkDayType = WorkDayType.Workday, WorkingPeriods = GetWorkingPeriods(date).ToList() };
            }
        }

        public static IEnumerable<WorkingPeriod> GetWorkingPeriods(DateTime date) {
            int periodCount = _rnd.Next(1, 3);

            for (int i = 1; i < periodCount + 1; i++) {
                (DateTime start, DateTime end) = GetStartEndTime(date, i, periodCount);

                yield return new WorkingPeriod { StartTime = start, EndTime = end };
            }
        }

        private static (DateTime start, DateTime end) GetStartEndTime(DateTime date, int current, int total) {
            if (total == 1) {
                return (date.Date.AddSeconds(_rnd.Next(6 * 60 * 60, 8 * 60 * 60)), date.Date.AddSeconds(_rnd.Next(15 * 60 * 60, 17 * 60 * 60)));
            }

            if (current == 1) {
                return (date.Date.AddSeconds(_rnd.Next(6 * 60 * 60, 8 * 60 * 60)), date.Date.AddSeconds(_rnd.Next(12 * 60 * 60, (int)(12.5 * 60 * 60))));
            }

            return (date.Date.AddSeconds(_rnd.Next((int)(12.5 * 60 * 60), 13 * 60 * 60)), date.Date.AddSeconds(_rnd.Next(15 * 60 * 60, 17 * 60 * 60)));
        }
    }
}