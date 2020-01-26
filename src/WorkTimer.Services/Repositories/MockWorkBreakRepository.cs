using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WorkTimer.Contracts;
using WorkTimer.Models;
#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously

namespace WorkTimer.Repositories {
    public class MockWorkBreakRepository : IWorkBreakRepository {

        private IEnumerable<WorkPeriod> Data => MockWorkPeriodRepository.Data.Where(x => x.IsBreak);

        public MockWorkBreakRepository() {

        }

        public async Task<IEnumerable<WorkPeriod>> FindByDate(DateTime date) {
            await Task.Delay(0);
            return Data.Where(x => x.Date == date.Date);
        }

        public async Task<IEnumerable<WorkPeriod>> GetAll() {
            await Task.Delay(0);
            return Data;
        }

        public async Task<IEnumerable<WorkPeriod>> GetIncomplete() {
            await Task.Delay(0);
            return Data.Where(x => !x.EndTime.HasValue);
        }
    }
}
#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously
