using Microsoft.AspNetCore.Identity;
using System;
using System.Threading.Tasks;
using WorkTimer.Api.Contracts;
using WorkTimer.Api.Models;
using WorkTimer.EF.Models;

namespace WorkTimer.Api.Services {
    public class AuthProvider : IAuthProvider {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;

        public AuthProvider(UserManager<AppUser> userManager,
                            SignInManager<AppUser> signInManager) {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public async Task<User> Login(LoginModel model) {
            if (model == null) {
                throw new ArgumentNullException(nameof(model));
            }

            var dbUser = await _userManager.FindByEmailAsync(model.Email);
            if (dbUser == null) {
                return null;
            }

            if (await _userManager.CheckPasswordAsync(dbUser, model.Password)) {
                return new User(dbUser.Id, dbUser.FirstName, dbUser.LastName, dbUser.Email, dbUser.UserName);
            }

            return null;
        }

        public async Task<User> Register(RegisterModel model) {
            if (model == null) {
                throw new ArgumentNullException(nameof(model));
            }

            var identityUser = new AppUser() {
                Email = model.Email,
                FirstName = model.FirstName,
                LastName = model.LastName,
                UserName = model.UserName,
            };

            var result = await _userManager.CreateAsync(identityUser, model.Password);

            if (result.Succeeded) {
                await _signInManager.SignInAsync(identityUser, isPersistent: false);

                var dbUser = await _userManager.FindByEmailAsync(model.Email);

                return new User(dbUser.Id, dbUser.FirstName, dbUser.LastName, dbUser.Email, dbUser.UserName);
            }

            return null;
        }

        public async Task<bool> UserExists(LoginModel model) {
            var dbUser = await _userManager.FindByEmailAsync(model.Email);
            return dbUser != null;
        }
    }
}
