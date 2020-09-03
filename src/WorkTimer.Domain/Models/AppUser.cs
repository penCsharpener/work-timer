using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace WorkTimer.Domain.Models {
    public class AppUser : IdentityUser<int> {

        public ICollection<Contract> Contracts { get; set; }
    }
}
