﻿using System.Collections.Generic;
using WorkTimer.Domain.Models;

namespace WorkTimer.Dev.Seeding {
    public static class Contracts {
        private static List<Contract> _appUsers = new List<Contract>() {
                    new Contract { Id = 1, UserId = 1, Name = "Pencsharpener AG Fulltime", Employer = "PenCsharpener AG", HoursPerWeek = 40, IsCurrent = false },
                    new Contract { Id = 2, UserId = 1, Name = "Pencsharpener AG part time", Employer = "PenCsharpener AG", HoursPerWeek = 25, IsCurrent = true  },
                    new Contract { Id = 3, UserId = 1, Name = "Employer Heaven Ldt Fulltime", Employer = "Employer Heaven Ldt", HoursPerWeek = 40, IsCurrent = false  }
                };

        public static List<Contract> GetEntities() {
            return _appUsers;
        }
    }
}
