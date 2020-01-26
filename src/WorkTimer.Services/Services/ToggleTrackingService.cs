using System;
using System.Threading.Tasks;
using WorkTimer.Contracts;

namespace WorkTimer.Services {
    public class ToggleTrackingService : StartTrackingBase {

        public ToggleTrackingService(IWorkingDayRepository dayRepo,
                                 IWorkPeriodRepository periodRepo,
                                 IWriterWorkPeriod writerWorkPeriod)
            : base(dayRepo, periodRepo, writerWorkPeriod) {
        }

        public override async Task ToggleTracking(DateTime dateTime, string? comment = null) {
            await Task.Delay(0);
            // check if there are any incomplete days

        }
    }
}
