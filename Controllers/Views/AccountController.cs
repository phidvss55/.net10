using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using webapi.Models;
using webapi.Models.Views;

namespace webapi.Controllers.Views
{
    
    [Route("account")]
    public class AccountController : Controller
    {
        private readonly SignInManager<AppUser> signInManager;
        private readonly UserManager<AppUser> userManager;
        private readonly RoleManager<IdentityRole> roleManager;

        public AccountController(SignInManager<AppUser> signInManager, UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            this.signInManager = signInManager;
            this.userManager = userManager;
            this.roleManager = roleManager;
        }

        [HttpGet("login")]
        public IActionResult Login()
        {
            return View("~/Views/Pages/Account/Login.cshtml");
        }

        [HttpPost("login")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View("~/Views/Pages/Account/Login.cshtml", model);
            }

            var result = await signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, lockoutOnFailure: false);
            if (result.Succeeded)
            {
                // return View("~/Views/Pages/Index");
                // return RedirectToAction("Index", "Home");
                return Redirect("~/"); // rezor pages
            }

            ModelState.AddModelError(string.Empty, "Invalid Login Attempt.");
            return View("~/Views/Pages/Account/Login.cshtml", model);
        }

        [HttpGet("register")]
        public IActionResult Register()
        {
            return View("~/Views/Pages/Account/Register.cshtml");
        }

        [HttpPost("register")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View("~/Views/Pages/Account/Register.cshtml", model);
            }

            var user = new AppUser()
            {
                FullName = model.Name,
                UserName = model.Email,
                NormalizedUserName = model.Email.ToUpper(),
                Email = model.Email,
                NormalizedEmail = model.Email.ToUpper()
            };

            var result = await userManager.CreateAsync(user, model.Password);

            if (result.Succeeded)
            {
                var roleExist = await roleManager.RoleExistsAsync("User");

                if (!roleExist)
                {
                    var role = new IdentityRole("User");
                    await roleManager.CreateAsync(role);
                }

                await userManager.AddToRoleAsync(user, "User");

                await signInManager.SignInAsync(user, isPersistent: false);
                return RedirectToAction("Login", "Account");
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }

            return View("~/Views/Pages/Account/Register.cshtml", model);
        }

        [HttpGet("verify-email")]
        public IActionResult VerifyEmail()
        {
            return View("~/Views/Pages/Account/VerifyEmail.cshtml");
        }

        [HttpPost("verify-email")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> VerifyEmail(VerifyEmailViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View("~/Views/Pages/Account/VerifyEmail.cshtml", model);
            }

            var user = await userManager.FindByNameAsync(model.Email);

            if (user == null)
            {
                ModelState.AddModelError("", "User not found!");
                return View("~/Views/Pages/Account/VerifyEmail.cshtml", model);
            }
            else
            {
                return RedirectToAction("ChangePassword", "Account", new { username = user.UserName });
            }
        }

        [HttpGet("change-password")]
        public IActionResult ChangePassword(string username)
        {
            if (string.IsNullOrEmpty(username))
            {
                return RedirectToAction("VerifyEmail", "Account");
            }

            return View("~/Views/Pages/Account/ChangePassword.cshtml", new ChangePasswordViewModel { Email = username });
        }

        [HttpPost("change-password")]
        public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Something went wrong");
                return View("~/Views/Pages/Account/ChangePassword.cshtml", model);
            }

            var user = await userManager.FindByNameAsync(model.Email);

            if(user == null)
            {
                ModelState.AddModelError("", "User not found!");
                return View("~/Views/Pages/Account/ChangePassword.cshtml", model);
            }

            var result = await userManager.RemovePasswordAsync(user);
            if (result.Succeeded)
            {
                result = await userManager.AddPasswordAsync(user, model.NewPassword);
                return RedirectToAction("Login", "Account");
            }
            else
            {
                foreach(var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }

                return View("~/Views/Pages/Account/ChangePassword.cshtml", model);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
    }
}