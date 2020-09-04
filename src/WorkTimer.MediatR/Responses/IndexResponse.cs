using System;
using System.Collections.Generic;
using System.Linq;
using WorkTimer.Domain.Models;

namespace WorkTimer.MediatR.Responses {
    public class IndexResponse {
        public IList<DisplayWorkDayModel> WorkDays { get; set; }
        public IList<WorkingPeriod> MostRecentWorkPeriods { get; set; }
        public TimeSpan TotalOverHours { get; set; }
        public bool HasOngoingWorkPeriod { get; set; }
    }

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
        public double ContractedHours { get; set; }

        public TimeSpan WorkHours { get; }
    }
}
