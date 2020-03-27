using Microsoft.AspNetCore.Identity;

namespace WorkTimer.EF.Models {
    public class AppUser : IdentityUser<int> {
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}
