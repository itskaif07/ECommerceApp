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

        // GET: Orders
        public async Task<IActionResult> OrderIndex()
        {
            var orders = _context.Orders.Include(o => o.Product);
            return View(await orders.ToListAsync());
        }



        [HttpGet]
        public async Task<IActionResult> OrderDetails(int? orderId, int productId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            // Log the initial request details
            _logger.LogInformation("Received OrderDetails request with orderId: {OrderId}, productId: {ProductId}, and userId: {UserId}", orderId, productId, userId);

            if (userId == null)
            {
                _logger.LogWarning("User ID is null - user is not logged in.");
                return RedirectToAction("Login", "Login");
            }

            // Check product details
            var product = await _context.Products.FindAsync(productId);
            if (product == null)
            {
                _logger.LogError("Product with ID {ProductId} not found.", productId);
                return NotFound("Product not found.");
            }
            else
            {
                _logger.LogInformation("Fetched product with ID {ProductId}. Product details: Name={ProductName}, Price={ProductPrice}", productId, product.Name, product.Price);
            }

            // Check user details
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                _logger.LogError("User with ID {UserId} not found.", userId);
                return NotFound("User not found.");
            }
            else
            {
                _logger.LogInformation("Fetched user with ID {UserId}. User details: Name={UserName}, Address={UserAddress}", userId, user.Name, user.Address);
            }

            // Check if the order exists for the current user
            var order = await _context.Orders.FirstOrDefaultAsync(o => o.OrderId == orderId && o.UserId == userId);

            if (order == null)
            {
                // Creating a new order if it does not exist
                order = new Order
                {
                    UserId = userId,
                    ProductId = productId,
                    OrderDate = DateTime.UtcNow,
                    Quantity = 1,
                    TotalPrice = product.Price,
                    Status = "Pending",
                    ShippingAddress = $"{user?.Address}, {user?.City}, {user?.State}, {user?.PinCode}",
                    TrackingNumber = Guid.NewGuid().ToString(),
                    PaymentStatus = "Unpaid",
                    PaymentMethod = "CashOnDelivery"
                };

                _context.Orders.Add(order);
                await _context.SaveChangesAsync();
                _logger.LogInformation("Created a new order with order ID {OrderId} for user {UserId}.", order.OrderId, userId);
            }
            else
            {
                _logger.LogInformation("Existing order found with order ID {OrderId} for user {UserId}.", order.OrderId, userId);
            }

            return View(order);
        }




        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> OrderDetails(Order order)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Invalid model state for order submission.");
                return View(order);
            }

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var existingUser = await _userManager.GetUserAsync(User);
            if (existingUser == null)
            {
                _logger.LogError("User with ID {UserId} not found.", userId);
                return NotFound("User not found.");
            }

            order.UserId = userId;
            order.OrderDate = DateTime.UtcNow;

            // Fetch product to calculate the total price
            var product = await _context.Products.FindAsync(order.ProductId);
            if (product == null)
            {
                _logger.LogError("Product with ID {ProductId} not found.", order.ProductId);
                return NotFound("Product not found.");
            }

            order.TotalPrice = order.Quantity * product.Price;
            order.ShippingAddress = $"{existingUser.Address}, {existingUser.City}, {existingUser.State}, {existingUser.PinCode}";
            order.Status = "Pending";
            order.TrackingNumber = Guid.NewGuid().ToString();
            order.PaymentStatus = "Pending";

            _context.Orders.Add(order);
            await _context.SaveChangesAsync();

            _logger.LogInformation("Created order for user {UserId} with ID {OrderId}.", userId, order.OrderId);
            return RedirectToAction("Index", "Home");
        }




        // POST: Orders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var order = await _context.Orders.FindAsync(id);
            if (order != null)
            {
                _context.Orders.Remove(order);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool OrderExists(int id)
        {
            return _context.Orders.Any(e => e.OrderId == id);
        }
    }
}
