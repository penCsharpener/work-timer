using System;
using System.Collections.Generic;

namespace WorkTimer.Domain.Models
{
    public class WorkWeek
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int DaysWorked { get; set; }
        public int DaysOffWork { get; set; }
        public double TotalOverhours { get; set; }
        public double TotalRequiredHours { get; set; }
        public double TotalHours { get; set; }
        public int WeekNumber { get; set; }
        public DateTime WeekStart { get; set; }
        public ICollection<WorkDay>? WorkDays { get; set; }
        public AppUser User { get; set; }
    }
}
