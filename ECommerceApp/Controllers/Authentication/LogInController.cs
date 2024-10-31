using ECommerceApp.Models;
using ECommerceApp.ViewModels.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace ECommerceApp.Controllers.Authentication
{
    public class LogInController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ILogger<LogInController> _logger;

        public LogInController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, ILogger<LogInController> logger)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger; 
        }

        public IActionResult LogIn()
        {
            _logger.LogInformation("Displaying the login page.");
            return View("~/Views/Authentication/LogIn.cshtml");
        }

        [HttpPost]
        public async Task<IActionResult> LogIn(LogIn model)
        {
            if (!ModelState.IsValid)
            {
                return View("~/Views/Authentication/LogIn.cshtml", model); 
            }

            ViewBag.EmailChangeWarning = TempData["EmailChangeWarning"];

            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                ModelState.AddModelError(string.Empty, "Invalid login attempt."); 
                return View("~/Views/Authentication/LogIn.cshtml", model);
            }

            var result = await _signInManager.PasswordSignInAsync(user, model.Password, model.RememberMe, lockoutOnFailure: false);

            if (result.Succeeded)
            {
                return RedirectToAction("Index", "Home");
            }

            // Handle specific login failure cases
            if (result.IsLockedOut)
            {
                ModelState.AddModelError(string.Empty, "User is locked out."); 
            }
            else if (result.IsNotAllowed)
            {
                ModelState.AddModelError(string.Empty, "User is not allowed to log in."); 
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Invalid password. Please try again.");
            }

            return View("~/Views/Authentication/LogIn.cshtml", model); 
        }





        public async Task<IActionResult> LogOut()
        {
            await _signInManager.SignOutAsync();
            _logger.LogInformation("User logged out.");
            return RedirectToAction("Index", "Home");
        }
    }
}
