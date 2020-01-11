using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkTimer.Contracts;
using WorkTimer.Models;

namespace WorkTimer.Repositories {
    public class MockStartTracking : IStartTracking {
        private readonly IWorkingDayRepository _dayRepo;
        private readonly IWorkPeriodRepository _periodRepo;

        public MockStartTracking(IWorkingDayRepository dayRepo,
                                 IWorkPeriodRepository periodRepo) {
            _dayRepo = dayRepo;
            _periodRepo = periodRepo;
        }

        public async Task StartTracking(DateTime dateTime) {
            // check if there are any incomplete days
            if ((await _periodRepo.GetIncomplete()).Any()) {

            } else {
                // if no incomplete days create a new day

            }

            var newDay = new WorkingDay() { Date = dateTime.Date };
            var newPeriod = new WorkPeriod() { StartTime = dateTime };

        }
    }
}
