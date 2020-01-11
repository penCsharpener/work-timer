﻿using Dapper.Contrib.Extensions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace WorkTimer.Models {
    public class WorkPeriod {
        public int Id { get; set; }
        public int WorkingDayId { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public string? Comment { get; set; }

        [Computed]
        public IEnumerable<WorkBreak> WorkBreaks { get; set; } = new List<WorkBreak>();
    }
}
