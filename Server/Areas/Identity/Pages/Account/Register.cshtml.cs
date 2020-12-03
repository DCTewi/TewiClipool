using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TewiClipool.Server.Data;
using TewiClipool.Server.Models;
using TewiClipool.Shared;
using TewiClipool.Shared.Models;

namespace TewiClipool.Server.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class RegisterModel : PageModel
    {
        private readonly SignInManager<AppUser> _signInManager;
        private readonly UserManager<AppUser> _userManager;
        private readonly ILogger<RegisterModel> _logger;
        private readonly ApplicationDbContext _db;
        private readonly IConfiguration _configuration;

        public RegisterModel(
            UserManager<AppUser> userManager,
            SignInManager<AppUser> signInManager,
            ILogger<RegisterModel> logger,
            ApplicationDbContext db,
            IConfiguration configuration)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
            _db = db;
            _configuration = configuration;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public string ReturnUrl { get; set; }

        public IList<AuthenticationScheme> ExternalLogins { get; set; }

        public class InputModel
        {
            [Required]
            [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
            [Display(Name = LocalStrings.RemotePage.RegisterToken)]
            public string RegisterToken { get; set; }

            [Required]
            [StringLength(50, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 1)]
            [Display(Name = LocalStrings.RemotePage.NewNickName)]
            public string NickName { get; set; }

            [Required]
            [StringLength(50, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 1)]
            [Display(Name = LocalStrings.RemotePage.NewUserName)]
            public string UserName { get; set; }

            [Required]
            [EmailAddress]
            [Display(Name = LocalStrings.RemotePage.NewEmail)]
            public string Email { get; set; }

            [Required]
            [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
            [DataType(DataType.Password)]
            [Display(Name = LocalStrings.RemotePage.NewPassword)]
            public string Password { get; set; }

            [DataType(DataType.Password)]
            [Display(Name = LocalStrings.RemotePage.ConfirmPassword)]
            [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
            public string ConfirmPassword { get; set; }
        }

        public async Task OnGetAsync(string returnUrl = null)
        {
            ReturnUrl = returnUrl;
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl ??= Url.Content("~/");
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
            if (ModelState.IsValid)
            {
                RegisterToken registerToken;
                try
                {
                    registerToken = _db.RegisterTokens.First(token => token.Token == Input.RegisterToken);
                    if (registerToken.Used)
                    {
                        ModelState.AddModelError(string.Empty, "Token is already used.");
                        return Page();
                    }
                }
                catch (InvalidOperationException)
                {
                    ModelState.AddModelError(string.Empty, "Invalid token.");
                    return Page();
                }

                var user = new AppUser
                {
                    NickName = Input.NickName,
                    UserName = Input.UserName,
                    Email = Input.Email,
                    RegisterToken = registerToken
                };

                var result = await _userManager.CreateAsync(user, Input.Password);
                if (result.Succeeded)
                {
                    registerToken.Used = true;
                    _db.RegisterTokens.Update(registerToken);
                    _logger.LogInformation("User created a new account with password.");

                    await _userManager.AddToRoleAsync(user, Literal.Roles.User);
                    user.IsUser = true;
                    _logger.LogInformation("Add user to role user.");

                    if (registerToken.Token == _configuration.GetValue<string>(Literal.AdminRegisterToken))
                    {
                        await _userManager.AddToRoleAsync(user, Literal.Roles.Admin);
                        await _userManager.AddToRoleAsync(user, Literal.Roles.Operator);
                        user.IsOperator = true;

                        _logger.LogInformation("Add user to role admin.");
                    }
                    _db.Users.Update(user);
                    await _db.SaveChangesAsync();

                    var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                    var callbackUrl = Url.Page(
                        "/Account/ConfirmEmail",
                        pageHandler: null,
                        values: new { area = "Identity", userId = user.Id, code = code, returnUrl = returnUrl },
                        protocol: Request.Scheme);

                    //await _emailSender.SendEmailAsync(Input.Email, "Confirm your email",
                    //    $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");

                    if (_userManager.Options.SignIn.RequireConfirmedAccount)
                    {
                        return RedirectToPage("RegisterConfirmation", new { email = Input.Email, returnUrl = returnUrl });
                    }
                    else
                    {
                        await _signInManager.SignInAsync(user, isPersistent: false);
                        return LocalRedirect(returnUrl);
                    }
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            // If we got this far, something failed, redisplay form
            return Page();
        }
    }
}
