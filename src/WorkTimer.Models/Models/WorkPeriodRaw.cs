using Dapper.Contrib.Extensions;

namespace WorkTimer.Models {
    [Table(nameof(WorkPeriod) + "s")]
    public class WorkPeriodRaw {
        public int Id { get; set; }
        public double StartTime { get; set; }
        public double? EndTime { get; set; }
        public int IsBreak { get; set; }
        public string? Comment { get; set; }
    }
}
