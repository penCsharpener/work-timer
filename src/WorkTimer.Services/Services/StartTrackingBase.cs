using System;
using System.Linq;
using System.Threading.Tasks;
using WorkTimer.Contracts;

namespace WorkTimer.Services {
    public class StartTrackingBase : IToggleTracking {

        protected readonly IWorkingDayRepository _dayRepo;
        protected readonly IWorkPeriodRepository _periodRepo;
        protected readonly IWriterWorkPeriod _writerWorkPeriod;

        public StartTrackingBase(IWorkingDayRepository dayRepo,
                                 IWorkPeriodRepository periodRepo,
                                 IWriterWorkPeriod writerWorkPeriod) {
            _dayRepo = dayRepo;
            _periodRepo = periodRepo;
            _writerWorkPeriod = writerWorkPeriod;
        }

        public virtual Task ToggleTracking(DateTime dateTime, bool isBreak, string? comment = null) {
            return Task.CompletedTask;
        }

        public async Task<bool> IncompletePeriodExists() {
            return (await _periodRepo.GetIncomplete()).Any();
        }

        public async Task<bool> TrackingExists(DateTime date) {
            await Task.Delay(0);
            return (await _dayRepo.FindByDate(date)) != null;
        }

        public async Task<bool> TrackingExistsForToday() {
            await Task.Delay(0);
            return true;
        }
    }
}
