using System;
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

        public async Task<IActionResult> CartsIndex(int id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var cartItems = await _context.Carts.Include(p => p.product).Where(w => w.UserId == userId).OrderByDescending(p => p.DateAdded).ToListAsync();

            var totalAmount = cartItems.Sum(p => p.product.DiscountedPrice * p.Quantity);

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

            return RedirectToAction("CartsIndex", "Carts");
        }


        public async Task<IActionResult> CartDetails(int id)
        {
            var product = await _context.Products
                .Include(p => p.Category)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (product == null)
            {
                return NotFound("Product not found.");
            }

            var user = await _userManager.GetUserAsync(User);

            if (user == null)
            {
                return Unauthorized("User not found");
            }

            bool isInWishList = await _context.Wishlists.AnyAsync(w => w.ProductId == id && w.UserId == user.Id);

            var viewModel = new ProductUserViewModel
            {
                Product = product,
                ApplicationUser = user,
                IsInWishlist = isInWishList
            };

            return View("~/Views/Carts/CartDetails.cshtml", viewModel);
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

            if(product.Quantity <= 1)
            {
                _context.Carts.Remove(product);
            }
            else
            {
                product.Quantity--;
            }

            await _context.SaveChangesAsync();

            return RedirectToAction("CartsIndex", "Carts");
        }

        [HttpPost]
        public async Task<IActionResult> AppendQuantity(int id)
        {
            var cartItem = await _context.Carts.FindAsync(id);

            if (cartItem == null)
            {
                return NotFound();
            }

            cartItem.Quantity++;

            await _context.SaveChangesAsync();

            return RedirectToAction("CartsIndex", "Carts");
        }


        private bool CartExists(int id)
        {
            return _context.Carts.Any(e => e.Id == id);
        }
    }
}
