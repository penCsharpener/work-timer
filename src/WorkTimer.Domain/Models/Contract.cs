﻿namespace WorkTimer.Domain.Models {
    public class Contract {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Employer { get; set; }
        public int HoursPerWeek { get; set; }
        public int UserId { get; set; }

        public AppUser User { get; set; }
    }
}
