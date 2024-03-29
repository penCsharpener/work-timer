﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using WorkTimer.Domain.Models;

namespace WorkTimer.Blazor.Areas.Identity.Pages.Account {
    [AllowAnonymous]
    public class LogoutModel : PageModel {
        private readonly ILogger<LogoutModel> _logger;
        private readonly SignInManager<AppUser> _signInManager;

        public LogoutModel(SignInManager<AppUser> signInManager, ILogger<LogoutModel> logger) {
            _signInManager = signInManager;
            _logger = logger;
        }

        public async Task<IActionResult> OnPost() {
            await _signInManager.SignOutAsync();
            _logger.LogInformation("User logged out.");

            return RedirectToPage("Login");
        }
    }
}