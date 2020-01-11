using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkTimer.Contracts;
using WorkTimer.Models;

namespace WorkTimer.Repositories {
    public class MockWorkingDayRepository : IWorkingDayRepository {

        private List<WorkingDay> data;

        public MockWorkingDayRepository() {
            data = new List<WorkingDay>() {
                new WorkingDay() { Id = 1, Date = new DateTime(2020,1,6) },
                new WorkingDay() { Id = 2, Date = new DateTime(2020,1,7) },
                new WorkingDay() { Id = 3, Date = new DateTime(2020,1,8) },
                new WorkingDay() { Id = 4, Date = new DateTime(2020,1,9) },
                new WorkingDay() { Id = 5, Date = new DateTime(2020,1,10) },
            };
        }

        public async Task<IEnumerable<WorkingDay>> FindByIds(IEnumerable<int> ids) {
            await Task.Delay(0);
            return data.Where(x => ids.Contains(x.Id));
        }

        public async Task<IEnumerable<WorkingDay>> GetAll() {
            await Task.Delay(0);
            return data;
        }
    }
}
