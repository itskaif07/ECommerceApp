using ECommerceApp.Data;
using ECommerceApp.Helpers;
using ECommerceApp.Models;
using ECommerceApp.ViewModels;
using ECommerceApp.ViewModels.Authentication;
using Humanizer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace ECommerceApp.Controllers.Authentication
{
    public class SignUpController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly EmailService _emailService;
        private readonly ApplicationDbContext _context;
        private readonly ILogger<SignUpController> _logger;

        public SignUpController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, EmailService emailService, ApplicationDbContext context, ILogger<SignUpController> logger)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _emailService = emailService;  // Assigning injected service
            _context = context;
            _logger = logger;
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


            // Check if the user already exists and is pending
            var existingUser = await _userManager.FindByEmailAsync(model.Email);
            if (existingUser != null && existingUser.IsPending)
            {
                // If the user is pending, redirect to OTP verification
                return RedirectToAction("VerifyOtp", new { userId = existingUser.Id });
            }

            var existingUserName = await _userManager.FindByNameAsync(model.UserName);
            if (existingUserName != null && existingUserName.IsPending)
            {
                // If the username is pending, redirect to OTP verification
                return RedirectToAction("VerifyOtp", new { userId = existingUserName.Id });
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
                EmailConfirmed = false,
                IsPending = true // Mark user as pending
            };

            var existingPhoneUser = await _userManager.Users.FirstOrDefaultAsync(u => u.PhoneNumber == model.PhoneNumber);
            if (existingPhoneUser != null)
            {
                ModelState.AddModelError(string.Empty, "Phone number is already registered.");
                return View("~/Views/Authentication/SignUp.cshtml", model);
            }


            var result = await _userManager.CreateAsync(user, model.Password);
            if (result.Succeeded)
            {
                var otp = new Random().Next(100000, 999999);  // Generate 6-digit OTP
                var createdAt = DateTime.Now;

                var userOtp = new userOtp
                {
                    otp = otp,
                    createdAt = createdAt,
                    userId = user.Id
                };

                _context.userOtps.Add(userOtp);
                await _context.SaveChangesAsync();

                string subject = "Your One Time Password for Sign Up";
                string body = $@"
                            <p><strong>{otp}</strong> is your one-time password for sign up.</p>
                            <p>Do not share it with anyone.</p>
                            <p><em>It is valid for only 5 minutes.</em></p>
                            <p>Thank you for signing up with us!</p>
                            <p>Best Regards,<br>ShopCart</p>
                        ";

                await _emailService.SendEmailAsync(user.Email, subject, body);

                return RedirectToAction("VerifyOtp", new { userId = user.Id });
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }

            return View("~/Views/Authentication/SignUp.cshtml", model);
        }



        public IActionResult VerifyOtp(string userId)
        {
            var model = new VerifyOtpViewModel
            {
                UserId = userId,
            };

            return View("~/Views/Authentication/VerifyOtp.cshtml", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> VerifyOtp(VerifyOtpViewModel model)
        {
            var otpRecord = await _context.userOtps.FirstOrDefaultAsync(u => u.userId == model.UserId && u.otp == int.Parse(model.Otp));

            if (otpRecord == null)
            {
                ModelState.AddModelError(string.Empty, "Invalid OTP.");
                return View("~/Views/Authentication/VerifyOtp.cshtml", model);
            }

            if (otpRecord.createdAt.AddMinutes(5) < DateTime.Now)
            {
                ModelState.AddModelError(string.Empty, "Expired OTP.");
                return View("~/Views/Authentication/VerifyOtp.cshtml", model);
            }

            var user = await _userManager.FindByIdAsync(model.UserId);

            if (user == null)
            {
                ModelState.AddModelError(string.Empty, "User not found.");
                return View("~/Views/Authentication/VerifyOtp.cshtml", model);
            }

            user.EmailConfirmed = true;
            user.IsPending = false;  
            var result = await _userManager.UpdateAsync(user);

            if (!result.Succeeded)
            {
                ModelState.AddModelError(string.Empty, "There was an error verifying the OTP.");
                return View("~/Views/Authentication/VerifyOtp.cshtml", model);
            }

            // Sign in the user only if EmailConfirmed is true
            if (!user.EmailConfirmed)
            {
                ModelState.AddModelError(string.Empty, "You need to verify your email before logging in.");
                return View("~/Views/Authentication/VerifyOtp.cshtml", model);
            }

            await _signInManager.SignInAsync(user, isPersistent: true);

            return RedirectToAction("Index", "Home");
        }


    }
}



