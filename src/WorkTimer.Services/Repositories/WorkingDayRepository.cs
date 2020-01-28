using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WorkTimer.Contracts;
using WorkTimer.Models;

namespace WorkTimer.Repositories {
    public class WorkingDayRepository : IWorkingDayRepository {
        private readonly IWorkPeriodRepository _repo;

        public WorkingDayRepository(IWorkPeriodRepository repo) {
            _repo = repo;
        }

        public async Task<WorkingDay> FindByDate(DateTime dateTime) {
            return new WorkingDay() {
                Date = dateTime,
                WorkPeriods = await _repo.FindByDate(dateTime)
            };
        }

        public async Task<IEnumerable<WorkingDay>> GetAll() {
            var periods = await _repo.GetAll();
            return periods.GroupBy(x => x.StartTime.Date).Select(x => new WorkingDay() { Date = x.Key, WorkPeriods = x.Select(y => y).ToList() });
        }
    }
}
