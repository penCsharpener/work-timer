using Dapper.Contrib.Extensions;
using System;
using System.ComponentModel.DataAnnotations;

namespace WorkTimer.Models {
    public class WorkPeriod {
        public int Id { get; set; }

        [Required]
        public DateTime StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public bool IsBreak { get; set; }
        public string? Comment { get; set; }
        public int ExpectedHours { get; set; } = 8;

        [Computed]
        public DateTime Date => StartTime.Date;

        [Computed]
        public TimeSpan WorkTime {
            get {
                if (EndTime.HasValue) {
                    return EndTime.Value - StartTime;
                }
                return DateTime.Now - StartTime;
            }
        }
    }
}
