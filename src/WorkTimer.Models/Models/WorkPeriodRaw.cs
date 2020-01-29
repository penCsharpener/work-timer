using Dapper.Contrib.Extensions;

namespace WorkTimer.Models {
    [Table(nameof(WorkPeriod) + "s")]
    public class WorkPeriodRaw {
        public int Id { get; set; }
        public string StartTime { get; set; } = "";
        public string? EndTime { get; set; }
        public int IsBreak { get; set; }
        public string? Comment { get; set; }
    }
}
