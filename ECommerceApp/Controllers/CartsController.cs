﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ECommerceApp.Data;
using ECommerceApp.Models;
using System.Security.Claims;
using Microsoft.CodeAnalysis;
using ECommerceApp.ViewModels;
using Microsoft.AspNetCore.Identity;
using ECommerceApp.Helpers;

namespace ECommerceApp.Controllers
{
    public class CartsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public CartsController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> CartsIndex(int? id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var cartItems = await _context.Carts
                .Include(c => c.product)
                .Where(c => c.UserId == userId)
                .OrderByDescending(c => c.DateAdded)
                .Select(c => new ProductUserViewModel
                {
                    Cart = c,
                    Product = c.product
                })
                .ToListAsync();

            var totalAmount = cartItems.Sum(item => item.Product.DiscountedPrice * item.Cart.Quantity);

            foreach (var item in cartItems)
            {
                item.DeliveryCharge = item.Product.DiscountedPrice >= 1000
                    ? new Random().Next(1, 101)
                    : 0;

                item.DeliveryDate = DateTime.Now.AddDays(new Random().Next(1, 8))
                    .AddHours(new Random().Next(11, 21))
                    .AddMinutes(new Random().Next(0, 60))
                    .AddSeconds(-DateTime.Now.Second);

                ViewBag.DeliveryDate = item.DeliveryDate;
                ViewBag.DeliveryCharge = item.DeliveryCharge;
            }

            ViewBag.TotalAmount = totalAmount.ToString("0.00");
          

            return View(cartItems);
        }



        public async Task<IActionResult> DeleteAll()
        {
            var cartItems = await _context.Carts.Include(p => p.product).ToListAsync();
            return View(cartItems);
        }


        [HttpPost]
        public async Task<IActionResult> DeleteAllConfirm()
        {
            var cartItems = await _context.Carts.Include(p => p.product).ToListAsync();

            _context.Carts.RemoveRange(cartItems);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index", "Home");
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CartAdd([Bind("Id,UserId,ProductId,Quantity,DateAdded")] Cart cart, int id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (userId == null)
            {
                return RedirectToAction("Login, Login");
            }

            var product = await _context.Products.FindAsync(id);

            if (product == null)
            {
                return NotFound("Product not found");
            }

            var cartItem = await _context.Carts
       .FirstOrDefaultAsync(c => c.ProductId == id && c.UserId == userId);

            if (cartItem != null)
            {
                cartItem.Quantity++;
            }
            else
            {
                cartItem = new Cart
                {
                    UserId = userId,
                    ProductId = id,
                    Quantity = 1,
                    DateAdded = DateTime.UtcNow
                };

                _context.Carts.Add(cartItem);
            }

            await _context.SaveChangesAsync();

            string actionResult = "add";

            var (message, notificationType) = NotificationHelper.GetNotificationMessage(actionResult);

            string formattedMessage = NotificationHelper.FormatNotification(message, notificationType);

            TempData["Notification"] = formattedMessage;

            return RedirectToAction("CartsIndex", "Carts");
        }



        public async Task<IActionResult> DeleteCart(int id)
        {
            var cart = await _context.Carts
                .Include(c => c.product)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (cart == null)
            {
                return NotFound();
            }

            return View(cart);
        }


        [HttpPost]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var cartItem = await _context.Carts.FindAsync(id);

            if (cartItem != null)
            {
                _context.Carts.Remove(cartItem);
                await _context.SaveChangesAsync();

                string actionResult = "remove";

                var (message, notificationType) = NotificationHelper.GetNotificationMessage(actionResult);

                string formattedMessage = NotificationHelper.FormatNotification(message, notificationType);
                TempData["Notification"] = formattedMessage;
            }

            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public async Task<IActionResult> ReductQuantity(int id)
        {
            var product = await _context.Carts.FindAsync(id);

            if (product == null)
            {
                return NotFound();
            }

            if (product.Quantity <= 1)
            {
                _context.Carts.Remove(product);

                string actionResult = "remove";

                var (message, notificationType) = NotificationHelper.GetNotificationMessage(actionResult);

                string formattedMessage = NotificationHelper.FormatNotification(message, notificationType);
                TempData["Notification"] = formattedMessage;
                
            }
            else
            {
                product.Quantity--;

                string actionResult = "decrease";

                var (message, notificationType) = NotificationHelper.GetNotificationMessage(actionResult);

                string formattedMessage = NotificationHelper.FormatNotification(message, notificationType);
                TempData["Notification"] = formattedMessage;
            }

            await _context.SaveChangesAsync();

            return RedirectToAction("CartsIndex", "Carts");
        }

        [HttpPost]
        public async Task<IActionResult> AppendQuantity(int id)
        {
            // Fetch cart item with product details
            var cartItem = await _context.Carts
                .Include(c => c.product)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (cartItem == null)
            {
                return NotFound("Cart item not found.");
            }

            // Check if product is null or stock is insufficient
            if (cartItem.product == null)
            {
                return NotFound("Product associated with cart item not found.");
            }

            string actionResult;
            if (cartItem.Quantity >= cartItem.product.Quantity)
            {
                actionResult = "insufficientstock";
            }
            else
            {
                // Increment quantity if stock allows
                cartItem.Quantity++;
                await _context.SaveChangesAsync();
                actionResult = "increase"; 
            }

            
            var (message, notificationType) = NotificationHelper.GetNotificationMessage(actionResult);

            string formattedMessage = NotificationHelper.FormatNotification(message, notificationType);
            TempData["Notification"] = formattedMessage;

            return RedirectToAction("CartsIndex", "Carts");
        }




        private bool CartExists(int id)
        {
            return _context.Carts.Any(e => e.Id == id);
        }
    }
}
