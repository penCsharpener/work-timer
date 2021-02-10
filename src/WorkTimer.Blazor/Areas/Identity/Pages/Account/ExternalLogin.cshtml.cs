﻿using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using WorkTimer.Domain.Models;
using SignInResult = Microsoft.AspNetCore.Identity.SignInResult;

namespace WorkTimer.Blazor.Areas.Identity.Pages.Account {
    [AllowAnonymous]
    public class ExternalLoginModel : PageModel {
        private readonly IEmailSender _emailSender;
        private readonly ILogger<ExternalLoginModel> _logger;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly UserManager<AppUser> _userManager;

        public ExternalLoginModel(
            SignInManager<AppUser> signInManager,
            UserManager<AppUser> userManager,
            ILogger<ExternalLoginModel> logger,
            IEmailSender emailSender) {
            _signInManager = signInManager;
            _userManager = userManager;
            _logger = logger;
            _emailSender = emailSender;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public string ProviderDisplayName { get; set; }

        public string ReturnUrl { get; set; }

        [TempData]
        public string ErrorMessage { get; set; }

        public IActionResult OnGetAsync() {
            return RedirectToPage("./Login");
        }

        public IActionResult OnPost(string provider, string returnUrl = null) {
            // Request a redirect to the external login provider.
            string? redirectUrl = Url.Page("./ExternalLogin", "Callback", new { returnUrl });
            AuthenticationProperties? properties = _signInManager.ConfigureExternalAuthenticationProperties(provider, redirectUrl);

            return new ChallengeResult(provider, properties);
        }

        public async Task<IActionResult> OnGetCallbackAsync(string returnUrl = null, string remoteError = null) {
            returnUrl = returnUrl ?? Url.Content("~/");

            if (remoteError != null) {
                ErrorMessage = $"Error from external provider: {remoteError}";

                return RedirectToPage("./Login", new { ReturnUrl = returnUrl });
            }

            ExternalLoginInfo? info = await _signInManager.GetExternalLoginInfoAsync();

            if (info == null) {
                ErrorMessage = "Error loading external login information.";

                return RedirectToPage("./Login", new { ReturnUrl = returnUrl });
            }

            // Sign in the user with this external login provider if the user already has a login.
            SignInResult? result = await _signInManager.ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey, false, true);

            if (result.Succeeded) {
                _logger.LogInformation("{Name} logged in with {LoginProvider} provider.", info.Principal.Identity.Name, info.LoginProvider);

                return LocalRedirect(returnUrl);
            }

            if (result.IsLockedOut) {
                return RedirectToPage("./Lockout");
            }

            // If the user does not have an account, then ask the user to create an account.
            ReturnUrl = returnUrl;
            ProviderDisplayName = info.ProviderDisplayName;

            if (info.Principal.HasClaim(c => c.Type == ClaimTypes.Email)) {
                Input = new InputModel { Email = info.Principal.FindFirstValue(ClaimTypes.Email) };
            }

            return Page();
        }

        public async Task<IActionResult> OnPostConfirmationAsync(string returnUrl = null) {
            returnUrl = returnUrl ?? Url.Content("~/");
            // Get the information about the user from the external login provider
            ExternalLoginInfo? info = await _signInManager.GetExternalLoginInfoAsync();

            if (info == null) {
                ErrorMessage = "Error loading external login information during confirmation.";

                return RedirectToPage("./Login", new { ReturnUrl = returnUrl });
            }

            if (ModelState.IsValid) {
                AppUser? user = new AppUser { UserName = Input.Email, Email = Input.Email };

                IdentityResult? result = await _userManager.CreateAsync(user);

                if (result.Succeeded) {
                    result = await _userManager.AddLoginAsync(user, info);

                    if (result.Succeeded) {
                        _logger.LogInformation("User created an account using {Name} provider.", info.LoginProvider);

                        string? userId = await _userManager.GetUserIdAsync(user);
                        string? code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                        code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));

                        string? callbackUrl = Url.Page(
                            "/Account/ConfirmEmail",
                            null,
                            new { area = "Identity", userId, code },
                            Request.Scheme);

                        await _emailSender.SendEmailAsync(Input.Email, "Confirm your email",
                                                          $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");

                        // If account confirmation is required, we need to show the link if we don't have a real email sender
                        if (_userManager.Options.SignIn.RequireConfirmedAccount) {
                            return RedirectToPage("./RegisterConfirmation", new { Input.Email });
                        }

                        await _signInManager.SignInAsync(user, false, info.LoginProvider);

                        return LocalRedirect(returnUrl);
                    }
                }

                foreach (var error in result.Errors) {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            ProviderDisplayName = info.ProviderDisplayName;
            ReturnUrl = returnUrl;

            return Page();
        }

        public class InputModel {
            [Required]
            [EmailAddress]
            public string Email { get; set; }
        }
    }
}