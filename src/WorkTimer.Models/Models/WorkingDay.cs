using Dapper.Contrib.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace WorkTimer.Models {
    public class WorkingDay {

        public DateTime Date { get; set; }

        [Computed]
        public IEnumerable<WorkPeriod> WorkPeriods { get; set; } = new List<WorkPeriod>();

        [Computed]
        public TimeSpan WorkTime => TimeSpan.FromSeconds(WorkPeriods.Where(x => !x.IsBreak).Sum(x => x.WorkTime.TotalSeconds));

        [Computed]
        public TimeSpan Overhours => WorkTime - TimeSpan.FromHours(8);

    }
}
