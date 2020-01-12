using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkTimer.Contracts;
using WorkTimer.Models;
#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously

namespace WorkTimer.Repositories {
    public class MockWriterWorkPeriod : IWriterWorkPeriod {

        public async Task Delete(WorkPeriod item) {
            var itemToRemove = MockWorkPeriodRepository.Data.FirstOrDefault(x => x.Id == item.Id);
            MockWorkPeriodRepository.Data.Remove(itemToRemove);
        }

        public async Task<WorkPeriod> Insert(WorkPeriod item) {
            var currentMaxId = MockWorkPeriodRepository.Data.Max(x => x.Id);
            item.Id = currentMaxId + 1;
            MockWorkPeriodRepository.Data.Add(item);
            return item;
        }

        public async Task<WorkPeriod> Insert(DateTime dateTime, string? comment = null) {
            return await Insert(new WorkPeriod() {
                StartTime = dateTime,
                Comment = comment
            });
        }

        public async Task<WorkPeriod> Update(WorkPeriod item, string sql) {
            return item;
        }

        public async Task<WorkPeriod> UpdateEndTime(int id, DateTime endTime) {
            var item = MockWorkPeriodRepository.Data.Find(x => x.Id == id);
            if (item == null) {
                throw new ArgumentOutOfRangeException($"{nameof(WorkPeriod)} with id {id} not found.");
            }
            item.EndTime = endTime;
            return item;
        }
    }
}
#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously
