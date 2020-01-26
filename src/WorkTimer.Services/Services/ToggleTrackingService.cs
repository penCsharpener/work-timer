using System;
using System.Linq;
using System.Threading.Tasks;
using WorkTimer.Contracts;
using WorkTimer.Models;

namespace WorkTimer.Services {
    public class ToggleTrackingService : StartTrackingBase {

        public ToggleTrackingService(IWorkingDayRepository dayRepo,
                                 IWorkPeriodRepository periodRepo,
                                 IWriterWorkPeriod writerWorkPeriod)
            : base(dayRepo, periodRepo, writerWorkPeriod) {
        }

        public override async Task ToggleTracking(DateTime dateTime, bool isBreak, string? comment = null) {
            await Task.Delay(0);
            // check if there are any incomplete periods
            var incomplete = await _periodRepo.GetIncomplete();
            if (incomplete.Any()) {
                if (incomplete.Count() == 1) {
                    var period = incomplete.FirstOrDefault();
                    if (!period.EndTime.HasValue) {
                        await _writerWorkPeriod.UpdateEndTime(period.Id, dateTime);
                    }
                }
            } else {
                var newPeriod = new WorkPeriod() {
                    IsBreak = isBreak,
                    Comment = comment,
                    StartTime = dateTime
                };
                await _writerWorkPeriod.Insert(newPeriod);
            }
        }
    }
}
