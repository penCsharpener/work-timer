using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkTimer.Contracts;
using WorkTimer.Models;

namespace WorkTimer.Services {
    public class MockStartTracking : StartTrackingBase {

        public MockStartTracking(IWorkingDayRepository dayRepo,
                                 IWorkPeriodRepository periodRepo,
                                 IWorkBreakRepository breakRepo,
                                 IWriterWorkingDay writerWorkingDay,
                                 IWriterWorkPeriod writerWorkPeriod,
                                 IWriterWorkBreak writerWorkBreak)
            : base(dayRepo, periodRepo, breakRepo, writerWorkingDay, writerWorkPeriod, writerWorkBreak) {
        }

        public override async Task ToggleTracking(DateTime dateTime, string? comment = null) {
            // check if there are any incomplete days

            var incompleteEntries = await _periodRepo.GetIncomplete();
            if (incompleteEntries.Any()) {
                // if yes filter it out, get the latest and continue with it
                if (incompleteEntries.OrderByDescending(x => x.StartTime).FirstOrDefault() is { } latest) {
                    var incompleteBreaks = await _breakRepo.FindByWorkPeriodIds(new int[] { latest.Id });

                    if (incompleteBreaks.Any() && incompleteBreaks.Count() == 1) {
                        var workBreak = incompleteBreaks.FirstOrDefault();
                        if (!workBreak.EndTime.HasValue) {
                            await _writerWorkBreak.UpdateEndTime(workBreak.Id, dateTime);
                        }
                    }

                    if (!latest.EndTime.HasValue) {
                        await _writerWorkPeriod.UpdateEndTime(latest.Id, dateTime);
                    }
                }
                
            } else {
                // if no incomplete days create a new day
                var newDay = new WorkingDay() { Date = dateTime.Date };
                var newPeriod = new WorkPeriod() { StartTime = dateTime };
                await _writerWorkingDay.Insert(dateTime.Date);
                await _writerWorkPeriod.Insert(dateTime, comment);
            }
        }

        public async Task ToggleBreak(DateTime dateTime) {
            // TODO: end any breaks when work period is ended
            //       in that case break and period end at the same time
        }
    }
}
