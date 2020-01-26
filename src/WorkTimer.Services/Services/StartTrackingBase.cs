using System;
using System.Linq;
using System.Threading.Tasks;
using WorkTimer.Contracts;
using WorkTimer.Repositories;

namespace WorkTimer.Services {
    public class StartTrackingBase : IToggleTracking {

        protected readonly IWorkingDayRepository _dayRepo;
        protected readonly IWorkPeriodRepository _periodRepo;
        protected readonly IWorkBreakRepository _breakRepo;
        protected readonly IWriterWorkingDay _writerWorkingDay;
        protected readonly IWriterWorkPeriod _writerWorkPeriod;
        protected readonly IWriterWorkBreak _writerWorkBreak;

        public StartTrackingBase(IWorkingDayRepository dayRepo,
                                 IWorkPeriodRepository periodRepo,
                                 IWorkBreakRepository breakRepo,
                                 IWriterWorkingDay writerWorkingDay,
                                 IWriterWorkPeriod writerWorkPeriod,
                                 IWriterWorkBreak writerWorkBreak) {
            _dayRepo = dayRepo;
            _periodRepo = periodRepo;
            _breakRepo = breakRepo;
            _writerWorkingDay = writerWorkingDay;
            _writerWorkPeriod = writerWorkPeriod;
            _writerWorkBreak = writerWorkBreak;
        }

        public virtual Task ToggleTracking(DateTime dateTime, string? comment = null) {
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
            return MockWorkingDayRepository.Data.Any(x => x.Date.Date == DateTime.Now.Date);
        }

        public virtual Task ToggleBreak(DateTime dateTime, string? comment = null) {
            return Task.CompletedTask;
        }
    }
}
