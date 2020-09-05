using System;
using System.Collections.Generic;
using System.Linq;

namespace WorkTimer.Domain.Models {
    public class WorkDay {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public WorkDayType WorkDayType { get; set; }
        public int ContractId { get; set; }

        public TimeSpan WorkTime => WorkingPeriods != null ? TimeSpan.FromSeconds(WorkingPeriods.Sum(x => x.WorkTime.TotalSeconds)) : new TimeSpan();
        public double ContractHoursPerDay => (Contract?.HoursPerWeek ?? 0) / (double)5;


        public Contract Contract { get; set; }
        public ICollection<WorkingPeriod> WorkingPeriods { get; set; }
    }
}
