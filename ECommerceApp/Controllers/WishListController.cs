using ECommerceApp.Data;
using ECommerceApp.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace ECommerceApp.Controllers
{
    public class WishListController : Controller
    {
        public readonly ApplicationDbContext _context;

        public WishListController(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> WishListIndex()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var wishlistItems = await _context.Wishlists.Include(p => p.Product).Where(w => w.UserId == userId).ToListAsync();

            return View(wishlistItems);
        }

        [HttpPost]
        public async Task<IActionResult> AddToWishlist(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound("Product does not exist.");
            }

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
            {
                return RedirectToAction("Login", "Login"); 
            }

            var existingEntry = await _context.Wishlists
                .FirstOrDefaultAsync(w => w.ProductId == id && w.UserId == userId);

            if (existingEntry != null)
            {
                return BadRequest("Product already in wishlist.");
            }

            // Create new wishlist entry
            var wishlistItem = new Wishlist
            {
                UserId = userId,
                ProductId = id
            };

            _context.Wishlists.Add(wishlistItem);
            await _context.SaveChangesAsync();

            return RedirectToAction("WishListIndex", "WishList"); 
        }

        [HttpPost]

        public async Task<IActionResult> RemoveItemWishList(int id)
        {
            var wishlistItem = await _context.Wishlists.FindAsync(id);

            if(wishlistItem != null)
            {
                _context.Wishlists.Remove(wishlistItem);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction("WishListIndex", "WishList");
        }


    }
}
