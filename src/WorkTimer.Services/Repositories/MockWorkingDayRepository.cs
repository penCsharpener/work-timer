using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkTimer.Contracts;
using WorkTimer.Models;

namespace WorkTimer.Repositories {
    public class MockWorkingDayRepository : IWorkingDayRepository {

        public static List<WorkingDay> Data = new List<WorkingDay>() {
                new WorkingDay() { Id = 1, Date = new DateTime(2020, 1, 6) },
                new WorkingDay() { Id = 2, Date = new DateTime(2020, 1, 7) },
                new WorkingDay() { Id = 3, Date = new DateTime(2020, 1, 8) },
                new WorkingDay() { Id = 4, Date = new DateTime(2020, 1, 9) },
                new WorkingDay() { Id = 5, Date = new DateTime(2020, 1, 10) },
            };

        public MockWorkingDayRepository() {
        }

        public async Task<WorkingDay?> FindByDate(DateTime dateTime) {
            await Task.Delay(0);
            var day = Data.Find(x => x.Date.Date == dateTime.Date);
            if (day == null) {
                return default;
            }
            day.WorkPeriods = MockWorkPeriodRepository.Data.Where(x => x.WorkingDayId == day.Id).ToList();
            if (day.WorkPeriods.Any()) {
                foreach (var period in day.WorkPeriods) {
                    period.WorkBreaks = MockWorkBreakRepository.Data.Where(x => x.WorkPeriodId == period.Id).ToList();
                }
            }
            return day;
        }

        public async Task<IEnumerable<WorkingDay>> FindByIds(IEnumerable<int> ids) {
            await Task.Delay(0);
            return Data.Where(x => ids.Contains(x.Id));
        }

        public async Task<IEnumerable<WorkingDay>> GetAll() {
            await Task.Delay(0);
            return Data;
        }

        public async Task<IEnumerable<WorkingDay>> GetIncomplete() {
            await Task.Delay(0);
            var list = new List<WorkPeriod>();
            foreach (var day in Data) {
                day.WorkPeriods = MockWorkPeriodRepository.Data.Where(x => x.WorkingDayId == day.Id).ToList();
            }
            return Data.Where(x => x.WorkPeriods.Any(y => !y.EndTime.HasValue));
        }
    }
}
