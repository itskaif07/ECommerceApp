using ECommerceApp.Models;
using ECommerceApp.ViewModels.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;


namespace ECommerceApp.Controllers.Authentication
{
    public class SignUpController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public SignUpController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public IActionResult SignUp()
        {
            return View("~/Views/Authentication/SignUp.cshtml");
        }

        [HttpPost]

        public async Task<IActionResult> SignUp(SignUp model)
        {
            if (!ModelState.IsValid)
            {
                return View("~/Views/Authentication/SignUp.cshtml", model);
            }

            var user = new ApplicationUser
            {
                Name = model.Name,
                UserName = model.UserName,
                Email = model.Email,
                Address = model.Address,
                City = model.City,
                State = model.State,
                PinCode = model.PinCode,
                PhoneNumber = model.PhoneNumber,
            };

            var result = await _userManager.CreateAsync(user, model.Password);

            if (result.Succeeded)
            {
                await _signInManager.SignInAsync(user, isPersistent: false);
                return RedirectToAction("Index", "Home");
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }

            return View("~/Views/Authentication/SignUp.cshtml", model);
        }
    }
}
