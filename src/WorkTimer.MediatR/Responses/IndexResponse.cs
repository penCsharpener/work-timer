using System;
using System.Collections.Generic;
using WorkTimer.Domain.Models;

namespace WorkTimer.MediatR.Responses {
    public class IndexResponse {
        public PagedResult<DisplayWorkDayModel> WorkDays { get; internal set; }
        public IList<WorkingPeriod> MostRecentWorkPeriods { get; internal set; }
        public TimeSpan TotalOverHours { get; internal set; }
        public bool HasOngoingWorkPeriod { get; internal set; }
        public string RedirectRoute { get; internal set; }
    }

    public class DisplayWorkDayModel : WorkDay {
        public DisplayWorkDayModel(WorkDay workDay) {
            Id = workDay.Id;
            Date = workDay.Date;
            WorkDayType = workDay.WorkDayType;
            ContractId = workDay.ContractId;
            Contract = workDay.Contract;
            WorkingPeriods = workDay.WorkingPeriods;
        }

        public TimeSpan Overhours { get; internal set; }
        public bool HasOngoingWorkingDay { get; internal set; }
        public double ContractedHours { get; internal set; }

        public TimeSpan WorkHours { get; internal set; }
    }
}
