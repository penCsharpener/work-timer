using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkTimer.Contracts;
using WorkTimer.Models;
#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously

namespace WorkTimer.Repositories {
    public class MockWriterWorkingDays : IWriterWorkingDay {


        public async Task Delete(WorkingDay item) {
            var itemToRemove = MockWorkingDayRepository.Data.FirstOrDefault(x => x.Id == item.Id);
            MockWorkingDayRepository.Data.Remove(itemToRemove);
        }

        public async Task<WorkingDay> Insert(WorkingDay item) {
            var currentMaxId = MockWorkingDayRepository.Data.Max(x => x.Id);
            item.Id = currentMaxId + 1;
            MockWorkingDayRepository.Data.Add(item);
            return item;
        }

        public async Task<WorkingDay> Insert(DateTime dateTime) {
            return await Insert(new WorkingDay() {
                Date = dateTime.Date,
            });
        }

        public async Task<WorkingDay> Update(WorkingDay item, string sql) {
            return item;
        }

    }
}
#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously
