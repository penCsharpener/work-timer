using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WorkTimer.Domain.Models;
using WorkTimer.Persistence.Data;

namespace WorkTimer.Dev.Seeding {
    public static class Users {

        private static string _pwd = "123123";
        private static List<AppUser> _appUsers;

        public static List<AppUser> GetUsers() {
            if (_appUsers == null) {
                _appUsers = new List<AppUser>() {
                    new AppUser("admin@gmail.com", _pwd) { Id = 1, },
                    new AppUser("john.doe@gmail.com", _pwd) { Id = 2, },
                    new AppUser("jane.doe@gmail.com", _pwd) { Id = 3, },
                };
            }
            return _appUsers;
        }

        public static async Task SeedUsers(this AppDbContext context, UserManager<AppUser> userManager) {
            if (!context.Users.Any()) {

                context.Users.AddRange(Users.GetUsers());
                context.SaveChanges();

                await userManager.AddToRoleAsync(Users.GetUsers()[0], "Admin");
                await userManager.AddToRoleAsync(Users.GetUsers()[0], "User");
                await userManager.AddToRoleAsync(Users.GetUsers()[1], "User");
                await userManager.AddToRoleAsync(Users.GetUsers()[2], "User");
            }
        }
    }
}
