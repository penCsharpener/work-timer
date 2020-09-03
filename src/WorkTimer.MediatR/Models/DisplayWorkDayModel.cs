using System;
using System.Linq;
using WorkTimer.Domain.Models;

namespace WorkTimer.MediatR.Models {
    public class DisplayWorkDayModel : WorkDay {
        public DisplayWorkDayModel(WorkDay workDay) {
            Id = workDay.Id;
            Date = workDay.Date;
            WorkDayType = workDay.WorkDayType;
            ContractId = workDay.ContractId;

            WorkHours = TimeSpan.FromSeconds(workDay.WorkingPeriods.Sum(x => x.WorkTime.TotalSeconds));
        }

        public TimeSpan Overhours => TimeSpan.FromSeconds(OverhoursInSeconds);
        public double OverhoursInSeconds { get; internal set; }
        public bool HasOngoingWorkingDay { get; internal set; }

        public TimeSpan WorkHours { get; }
    }
}
