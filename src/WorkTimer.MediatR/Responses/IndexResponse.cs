using System;
using System.Collections.Generic;
using WorkTimer.Domain.Models;
using WorkTimer.MediatR.Models;

namespace WorkTimer.MediatR.Responses {
    public class IndexResponse {
        public IList<DisplayWorkDayModel> WorkDays { get; set; }
        public IList<WorkingPeriod> MostRecentWorkPeriods { get; set; }
        public TimeSpan TotalOverHours { get; set; }
        public bool HasOngoingWorkPeriod { get; set; }
    }
}
