using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.Json;
using System.Threading.Tasks;
using WorkTimer.Domain.Models;

namespace WorkTimer.Blazor.Areas.Identity.Pages.Account.Manage {
    public class DownloadPersonalDataModel : PageModel {
        private readonly ILogger<DownloadPersonalDataModel> _logger;
        private readonly UserManager<AppUser> _userManager;

        public DownloadPersonalDataModel(
            UserManager<AppUser> userManager,
            ILogger<DownloadPersonalDataModel> logger) {
            _userManager = userManager;
            _logger = logger;
        }

        public async Task<IActionResult> OnPostAsync() {
            AppUser? user = await _userManager.GetUserAsync(User);

            if (user == null) {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            _logger.LogInformation("User with ID '{UserId}' asked for their personal data.", _userManager.GetUserId(User));

            // Only include personal data for download
            Dictionary<string, string>? personalData = new Dictionary<string, string>();

            IEnumerable<PropertyInfo>? personalDataProps = typeof(AppUser).GetProperties().Where(
                prop => Attribute.IsDefined(prop, typeof(PersonalDataAttribute)));

            foreach (var p in personalDataProps) {
                personalData.Add(p.Name, p.GetValue(user)?.ToString() ?? "null");
            }

            IList<UserLoginInfo>? logins = await _userManager.GetLoginsAsync(user);

            foreach (var l in logins) {
                personalData.Add($"{l.LoginProvider} external login provider key", l.ProviderKey);
            }

            Response.Headers.Add("Content-Disposition", "attachment; filename=PersonalData.json");

            return new FileContentResult(JsonSerializer.SerializeToUtf8Bytes(personalData), "application/json");
        }
    }
}