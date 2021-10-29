using System;

namespace WorkTimer.Domain.Models
{
    public class OverHoursCompensation
    {
        public int Id { get; set; }
        public int ContractId { get; set; }
        public DateTime DateFrom { get; set; }
        public DateTime DateTo { get; set; }
        public double OverHours { get; set; }
        public string Comment { get; set; }

        public Contract Contract { get; set; }
    }
}
