using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using WorkTimer.Domain.Models;

namespace WorkTimer.Blazor.Areas.Identity.Pages.Account.Manage {
    public class PersonalDataModel : PageModel {
        private readonly ILogger<PersonalDataModel> _logger;
        private readonly UserManager<AppUser> _userManager;

        public PersonalDataModel(
            UserManager<AppUser> userManager,
            ILogger<PersonalDataModel> logger) {
            _userManager = userManager;
            _logger = logger;
        }

        public async Task<IActionResult> OnGet() {
            AppUser? user = await _userManager.GetUserAsync(User);

            if (user == null) {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            return Page();
        }
    }
}