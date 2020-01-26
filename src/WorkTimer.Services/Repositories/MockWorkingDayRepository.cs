using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WorkTimer.Contracts;
using WorkTimer.Models;

namespace WorkTimer.Repositories {
    public class MockWorkingDayRepository : IWorkingDayRepository {

        private List<WorkPeriod> Data => MockWorkPeriodRepository.Data;

        public MockWorkingDayRepository() {

        }

        public async Task<WorkingDay?> FindByDate(DateTime dateTime) {
            await Task.Delay(0);
            var day = Fill(Data).FirstOrDefault(x => x.Date.Date == dateTime.Date);
            return day ?? (default);
        }

        public async Task<IEnumerable<WorkingDay>> GetAll() {
            await Task.Delay(0);
            return Fill(Data);
        }

        private static IEnumerable<WorkingDay> Fill(IEnumerable<WorkPeriod> list) {
            foreach (var item in list.GroupBy(x => x.Date)) {
                yield return new WorkingDay() {
                    Date = item.Key,
                    WorkPeriods = item.Select(x => x).ToList(),
                };
            }
        }
    }
}
