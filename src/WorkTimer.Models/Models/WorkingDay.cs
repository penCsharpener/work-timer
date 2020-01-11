using Dapper.Contrib.Extensions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace WorkTimer.Models {
    public class WorkingDay {

        public int Id { get; set; }
        public DateTime Date { get; set; }

        [Computed]
        public IEnumerable<WorkPeriod> WorkPeriods { get; set; } = new List<WorkPeriod>();
    }
}
