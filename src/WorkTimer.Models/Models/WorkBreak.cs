using System;
using System.Collections.Generic;
using System.Text;

namespace WorkTimer.Models {
    public class WorkBreak {
        public int Id { get; set; }
        public int WorkPeriodId { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
        public string? Comment { get; set; }
    }
}
