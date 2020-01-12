using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkTimer.Contracts;
using WorkTimer.Models;
#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously

namespace WorkTimer.Repositories {
    public class MockWorkBreakRepository : IWorkBreakRepository {

        public static List<WorkBreak> Data = new List<WorkBreak>() {
            new WorkBreak() { Id = 1, WorkPeriodId = 10, StartTime = new DateTime(2020, 1, 10, 14, 11, 14), EndTime = new DateTime(2020, 1, 10, 14, 13, 55), Comment = "stretching my legs" }
        };

        public MockWorkBreakRepository() {

        }

        public async Task<IEnumerable<WorkBreak>> FindByWorkPeriodIds(IEnumerable<int> workPeriodIds) {
            await Task.Delay(0);
            return Data.Where(x => workPeriodIds.Contains(x.WorkPeriodId));
        }

        public async Task<IEnumerable<WorkBreak>> GetAll() {
            await Task.Delay(0);
            return Data;
        }

        public async Task<IEnumerable<WorkBreak>> GetIncomplete() {
            await Task.Delay(0);
            return Data.Where(x => !x.EndTime.HasValue);
        }
    }
}
#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously
