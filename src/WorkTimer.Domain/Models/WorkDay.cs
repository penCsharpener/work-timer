using System;
using System.Collections.Generic;

namespace WorkTimer.Domain.Models
{
    public class WorkDay
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public WorkDayType WorkDayType { get; set; }
        public int ContractId { get; set; }
        public double TotalHours { get; set; }
        public int? WorkWeekId { get; set; }
        public int? WorkMonthId { get; set; }

        public Contract Contract { get; set; }
        public WorkWeek? WorkWeek { get; set; }
        public WorkMonth? WorkMonth { get; set; }
        public ICollection<WorkingPeriod> WorkingPeriods { get; set; }
    }
}