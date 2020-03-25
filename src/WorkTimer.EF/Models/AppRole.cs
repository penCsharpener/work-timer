using Microsoft.AspNetCore.Identity;

namespace WorkTimer.EF.Models {
    public class AppRole : IdentityRole<int> {
        public AppRole() { }

        public AppRole(string roleName) : base(roleName) { }
    }
}
