using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ECommerceApp.Data;
using ECommerceApp.Models;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using NuGet.ProjectModel;

namespace ECommerceApp.Controllers
{
    public class OrdersController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger<OrdersController> _logger;

        public OrdersController(ApplicationDbContext context, UserManager<ApplicationUser> userManager, ILogger<OrdersController> logger)
        {
            _context = context;
            _userManager = userManager;
            _logger = logger;
        }

        public async Task<IActionResult> OrderIndex()
        {
            var user = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var orders = await _context.Orders.Include(o => o.Product).Where(u => u.UserId == user).OrderByDescending(o => o.OrderDate).ToListAsync();

            ViewBag.TotalAmount = orders.Sum(o => o.TotalPrice);
            return View(orders);
        }


        [HttpPost]
        public async Task<IActionResult> DeleteAll()
        {
            var orderItem = await _context.Orders.Include(p => p.Product).ToListAsync();

            _context.Orders.RemoveRange(orderItem);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public async Task<IActionResult> PlaceOrder()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var user = await _userManager.GetUserAsync(User);


            // Retrieve all cart items for the user
            var cartItems = await _context.Carts
                .Include(c => c.product)
                .Where(c => c.UserId == userId)
                .ToListAsync();

            if (!cartItems.Any())
            {
                return RedirectToAction("CartsIndex", "Carts");
            }

            // Process each cart item as an order
            foreach (var cartItem in cartItems)
            {
                var addressParts = new List<string> { user.Address, user.City, user.State, user.PinCode };
                var shippingAddress = string.Join(", ", addressParts.Where(part => !string.IsNullOrEmpty(part)));

                var order = new Order
                {
                    UserId = userId,
                    ProductId = cartItem.ProductId,
                    Quantity = cartItem.Quantity,
                    TotalPrice = cartItem.product.DiscountedPrice * cartItem.Quantity,
                    OrderDate = DateTime.UtcNow,
                    TrackingNumber = Guid.NewGuid().ToString(),
                    ShippingAddress = shippingAddress,
                    PaymentMethod = "CashOnDelivery",

                }; 

                _context.Orders.Add(order);
            }

            // Clear the user's cart after placing the order
            _context.Carts.RemoveRange(cartItems);

            // Save changes to the database
            await _context.SaveChangesAsync();

            return RedirectToAction("CartsIndex", "Carts");
        }

        [HttpGet("OrderDetails")]
        public async Task<IActionResult> OrderDetails(int? orderId, int productId, int? quantity = null)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (userId == null)
            {
                return RedirectToAction("Login", "Login");
            }

            var product = await _context.Products.FindAsync(productId);
            if (product == null)
            {
                return NotFound("Product not found.");
            }

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound("User not found.");
            }

            // Agar existing order already hai to use fetch karte hain
            var order = await _context.Orders
                .FirstOrDefaultAsync(o => o.UserId == userId && o.ProductId == productId && o.OrderId == orderId);

            // Agar order nahi hai to naya order banate hain
            if (order == null)
            {
                
                int finalQuantity = quantity ?? 1;
                var addressParts = new List<string> { user.Address, user.City, user.State, user.PinCode };
                var shippingAddress = string.Join(", ", addressParts.Where(part => !string.IsNullOrEmpty(part)));

                order = new Order
                {
                    UserId = userId,
                    ProductId = productId,
                    Quantity = finalQuantity,
                    OrderDate = DateTime.UtcNow,
                    TotalPrice = product.DiscountedPrice * finalQuantity, 
                    Status = "Pending",
                    ShippingAddress = shippingAddress,
                    TrackingNumber = Guid.NewGuid().ToString(),
                    PaymentStatus = "Unpaid",
                    PaymentMethod = "CashOnDelivery",
                    Product = product,
                    ApplicationUser = user
                };

                // New order ko database mein add karte hain
                _context.Orders.Add(order);
                await _context.SaveChangesAsync();
            }

            return View(order);
        }

        [HttpPost("OrderDetails")]
        public async Task<IActionResult> OrderDetails(Order order)
        {
            if (!ModelState.IsValid)
            {
                return View(order);
            }

            var product = await _context.Products.FindAsync(order.ProductId);
            if (product == null)
            {
                ModelState.AddModelError("", "Product not found.");
                return View(order);
            }

            // Ensure quantity has a valid value; default to 1 if null or 0
            int quantity = order.Quantity > 0 ? order.Quantity : 1;

            var existingOrder = await _context.Orders
                                .FirstOrDefaultAsync(o => o.UserId == order.UserId && o.ProductId == order.ProductId);

            // Ensure the user is associated with the order if it's a new order
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                ModelState.AddModelError("", "User not found.");
                return View(order);
            }

            if (existingOrder != null)
            {
                existingOrder.Quantity = quantity;
                existingOrder.TotalPrice = quantity * product.DiscountedPrice;
                existingOrder.ShippingAddress = order.ShippingAddress;
                existingOrder.TrackingNumber = Guid.NewGuid().ToString();
                existingOrder.PaymentMethod = order.PaymentMethod;
                existingOrder.ApplicationUser = user; 
                product.Quantity -= quantity;

                _context.Update(product);
                _context.Update(existingOrder);
            }
            else
            {
                order.Quantity = quantity;
                order.TotalPrice = quantity * product.DiscountedPrice;
                order.TrackingNumber = Guid.NewGuid().ToString();
                order.ApplicationUser = user; 

                product.Quantity -= quantity;

                _context.Add(order);
                _context.Update(product);
            }

            await _context.SaveChangesAsync();

            return RedirectToAction("Index", "Home", new { orderId = order.OrderId });
        }


        public async Task<IActionResult> ProductOrderDetails(int orderId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var order = await _context.Orders
                .Include(o => o.ApplicationUser)  // Ensure ApplicationUser is included
                .Include(o => o.Product)
                .ThenInclude(p => p.Category)
                .FirstOrDefaultAsync(o => o.OrderId == orderId && o.UserId == userId);

            if (order == null)
            {
                return NotFound("Order not found or user is not authorized to view this order");
            }

            // Debugging line to check if ApplicationUser is loaded
            if (order.ApplicationUser == null)
            {
                Console.WriteLine("No ApplicationUser associated with this order.");
            }

            return View(order);
        }





        public async Task<IActionResult> DeleteOrder(int orderId)
        {

            var orderItem = await _context.Orders.Include(p => p.Product).FirstOrDefaultAsync(o => o.OrderId == orderId);

            if (orderItem == null)
            {
                return NotFound();
            }

            return View(orderItem);
        }

        public async Task<IActionResult> ConfirmDeleteOrder(int orderId)
        {
            var orderItem = await _context.Orders.FindAsync(orderId);

            if (orderItem == null)
            {
                return NotFound();
            }

            _context.Orders.Remove(orderItem);
            await _context.SaveChangesAsync();

            return RedirectToAction("OrderIndex", "Orders");
        }

        private bool OrderExists(int id)
        {
            return _context.Orders.Any(e => e.OrderId == id);
        }
    }
}
