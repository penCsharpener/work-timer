using System;

namespace WorkTimer.Domain.Models {
    public class WorkingPeriod {
        public int Id { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public string? Comment { get; set; }
        public int WorkDayId { get; set; }

        public WorkDay WorkDay { get; set; }
    }
}