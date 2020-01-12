using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkTimer.Contracts;
using WorkTimer.Models;
#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously

namespace WorkTimer.Repositories {
    public class MockWriterWorkBreak : IWriterWorkBreak {

        public async Task Delete(WorkBreak item) {
            var itemToRemove = MockWorkBreakRepository.Data.FirstOrDefault(x => x.Id == item.Id);
            MockWorkBreakRepository.Data.Remove(itemToRemove);
        }

        public async Task<WorkBreak> Insert(DateTime dateTime, string? comment = null) {
            return await Insert(new WorkBreak() {
                StartTime = dateTime,
                Comment = comment
            });
        }

        public async Task<WorkBreak> Insert(WorkBreak item) {
            var currentMaxId = MockWorkBreakRepository.Data.Max(x => x.Id);
            item.Id = currentMaxId + 1;
            MockWorkBreakRepository.Data.Add(item);
            return item;
        }

        public async Task<WorkBreak> Update(WorkBreak item, string sql) {
            return item;
        }
    }
}
#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously
