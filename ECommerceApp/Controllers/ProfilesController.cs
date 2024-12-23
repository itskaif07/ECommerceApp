﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ECommerceApp.Data;
using ECommerceApp.ViewModels;
using Microsoft.AspNetCore.Identity;
using ECommerceApp.Models;
using System.Security.Claims;

namespace ECommerceApp.Controllers
{
    public class ProfilesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ProfilesController(ApplicationDbContext context, SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _signInManager = signInManager;
            _userManager = userManager;
            _webHostEnvironment = webHostEnvironment;
        }

        // GET: Profiles
        public async Task<IActionResult> ProfileIndex()
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("LogIn", "LogIn");
            }

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = await _userManager.FindByIdAsync(userId);
            return View(user);
        }

        // GET: Profiles/Edit/5
        [HttpGet]
        public async Task<IActionResult> ProfileEdit(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            return View(user);
        }


        [HttpPost]
        public async Task<IActionResult> ProfileEdit(ApplicationUser model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByIdAsync(model.Id);
                if (user != null)
                {
                    bool emailChanged = user.Email != model.Email;

                    user.Name = model.Name;
                    user.UserName = model.UserName;
                    user.Email = model.Email;
                    user.PhoneNumber = model.PhoneNumber;
                    user.Address = model.Address;
                    user.City = model.City;
                    user.State = model.State;
                    user.PinCode = model.PinCode;

                    var result = await _userManager.UpdateAsync(user);
                    if (result.Succeeded)
                    {
                        // Sign out the user if the email was changed
                        if (emailChanged)
                        {
                            await _signInManager.SignOutAsync();
                            TempData["EmailChangeWarning"] = "Email changed. Please log in with your new email.";
                            return RedirectToAction("Login", "Login"); 
                        }

                        return RedirectToAction("ProfileIndex", "Profiles");
                    }

                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }
            }
            return View(model);
        }


        [HttpGet]
        public async Task<IActionResult> ProfileDelete(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            return View(user); 
        }


        [HttpPost]
        public async Task<IActionResult> ProfileDeleteConfirmed(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            var result = await _userManager.DeleteAsync(user);
            if (result.Succeeded)
            {
                await _signInManager.SignOutAsync();
                return RedirectToAction("Index", "Home");
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }

            return View("Error");
        }

        public async Task<IActionResult> AllProfiles()
        {
            var profiles = await _userManager.Users.ToListAsync();
            return View(profiles);
        }

        public async Task<IActionResult> ProfileDetails(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return NotFound();
            }

            var profile = await _userManager.FindByIdAsync(id);

            if(profile == null)
            {
                return NotFound();
            }

            var roles = await _userManager.GetRolesAsync(profile);
            var role = roles.FirstOrDefault(); 

            ViewBag.UserRole = role;

            return View(profile);           
        }

        public async Task<IActionResult> AdminsList()
        {
            var admins = await _userManager.GetUsersInRoleAsync("SiteOwner");

            return View(admins);
        }

        [HttpGet]

        public async Task<IActionResult> AssignAdmin(string id)
        {
            var admin = await _userManager.FindByIdAsync(id);

            if(admin == null)
            {
                return NotFound();
            }

            return View(admin);
        }

        [HttpPost] 

        public async Task<IActionResult> ConfirmAssignAdmin(string id)
        {

            var admin = await _userManager.FindByIdAsync(id);

            if(admin == null)
            {
                return NotFound();
            }


            var result = await _userManager.AddToRoleAsync(admin, "SiteOwner");

            if (result.Succeeded)
            {
                return RedirectToAction("Index", "Home");
            }

            foreach(var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }

            return View(admin);
        }

        public async Task<IActionResult> AssignUser(string id)
        {
            var user = await _userManager.FindByIdAsync(id);

            if(user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        [HttpPost]
        public async Task<IActionResult> ConfirmAssignUser(string id)
        {
            var user = await _userManager.FindByIdAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            var result = await _userManager.RemoveFromRoleAsync(user, "SiteOwner"); 

            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(user, "User"); 

                return RedirectToAction("Index", "Home", new { id = user.Id });
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }

            return View(user); // Show the same view again with error messages if needed
        }


    }
}
