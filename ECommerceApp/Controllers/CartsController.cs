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

namespace ECommerceApp.Controllers
{
    public class CartsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CartsController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> CartsIndex(int id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var cartItems = await _context.Carts.Include(p => p.product).Where(w => w.UserId == userId).OrderByDescending(p => p.DateAdded).ToListAsync();

            return View(cartItems);
        }

        // GET: Carts/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cart = await _context.Carts
                .Include(c => c.product)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (cart == null)
            {
                return NotFound();
            }

            return View(cart);
        }

        // GET: Carts/Create
        public IActionResult Create()
        {
            ViewData["ProductId"] = new SelectList(_context.Products, "Id", "Name");
            return View();
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

        // GET: Carts/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cart = await _context.Carts.FindAsync(id);
            if (cart == null)
            {
                return NotFound();
            }
            ViewData["ProductId"] = new SelectList(_context.Products, "Id", "Name", cart.ProductId);
            return View(cart);
        }

        // POST: Carts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,UserId,ProductId,Quantity,DateAdded")] Cart cart)
        {
            if (id != cart.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(cart);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CartExists(cart.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["ProductId"] = new SelectList(_context.Products, "Id", "Name", cart.ProductId);
            return View(cart);
        }

        // GET: Carts/Delete/5
        public async Task<IActionResult> DeleteCart(int id)
        {
            var cart = await _context.Carts
                .Include(c => c.product)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (cart == null)
            {
                return NotFound();
            }

            return View(cart); // Displays the confirmation view
        }



        // POST: Carts/Delete/5
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

        private bool CartExists(int id)
        {
            return _context.Carts.Any(e => e.Id == id);
        }
    }
}
