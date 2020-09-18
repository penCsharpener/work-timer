using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace WorkTimer.Domain.Models {
    public class AppUser : IdentityUser<int> {

        public ICollection<Contract>? Contracts { get; set; }

        public AppUser() { }

        public AppUser(string email, string password) {
            UserName = Email = email;
            NormalizedUserName = NormalizedEmail = email.ToUpper();
            LockoutEnabled = false;
            EmailConfirmed = true;
            SecurityStamp = System.Guid.NewGuid().ToString();
            PasswordHash = new PasswordHasher<AppUser>().HashPassword(this, password);
        }
    }
}
