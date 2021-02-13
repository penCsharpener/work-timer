using System.Collections.Generic;

namespace WorkTimer.Domain.Models
{
    public class WorkMonth
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int DaysWorked { get; set; }
        public int DaysOffWork { get; set; }
        public double TotalOverhours { get; set; }
        public double TotalHours { get; set; }
        public int Year { get; set; }
        public int Month { get; set; }
        public ICollection<WorkDay>? WorkDays { get; set; }
    }
}
