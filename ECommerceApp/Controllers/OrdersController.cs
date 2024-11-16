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
using static NuGet.Packaging.PackagingConstants;

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


            var cartItems = await _context.Carts
                .Include(c => c.product)
                .Where(c => c.UserId == userId)
                .ToListAsync();

            if (!cartItems.Any())
            {
                return RedirectToAction("CartsIndex", "Carts");
            }


            foreach (var cartItem in cartItems)
            {
                var addressParts = new List<string> { user.Address, user.City, user.State, user.PinCode };
                var shippingAddress = string.Join(", ", addressParts.Where(part => !string.IsNullOrEmpty(part)));

                var order = new Order
                {
                    UserId = userId,
                    ProductId = cartItem.ProductId,
                    Quantity = cartItem.Quantity,
                    TotalPrice = (cartItem.product.DiscountedPrice + cartItem.product.DeliveryCharge ?? 0) * cartItem.Quantity,
                    OrderDate = DateTime.UtcNow,
                    TrackingNumber = Guid.NewGuid().ToString(),
                    ShippingAddress = shippingAddress,
                    PaymentMethod = "CashOnDelivery",

                };

                _context.Orders.Add(order);
            }

            _context.Carts.RemoveRange(cartItems);

            await _context.SaveChangesAsync();

            return RedirectToAction("CartsIndex", "Carts");
        }

        [HttpGet]
        public async Task<IActionResult> OrderDetails(int? orderId, int productId)
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


            var order = await _context.Orders
                .FirstOrDefaultAsync(o => o.UserId == userId && o.ProductId == productId && o.OrderId == orderId);

            Random random = new Random();

            bool isFreeDelivery = random.Next(0, 10) < 8;


            if (order == null)
            {
                order = new Order
                {
                    UserId = userId,
                    ProductId = productId,
                    OrderDate = DateTime.UtcNow,
                    TotalPrice = product.DiscountedPrice + (product.DeliveryCharge ?? 0),
                    Status = "Pending",
                    ShippingAddress = $"{user.Address}, {user.City}, {user.State}, {user.PinCode}",
                    TrackingNumber = Guid.NewGuid().ToString(),
                    PaymentStatus = "Unpaid",
                    PaymentMethod = "CashOnDelivery",
                    Product = product,
                    ApplicationUser = user
                };
            }

            return View(order);
        }


        [HttpPost]
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

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound("User not found.");
            }

            var existingOrder = await _context.Orders
                .FirstOrDefaultAsync(o => o.UserId == order.UserId && o.ProductId == order.ProductId);

            if (existingOrder != null)
            {

                existingOrder.Quantity = order.Quantity;
                existingOrder.TotalPrice = order.Quantity * (product.DiscountedPrice + (product.DeliveryCharge ?? 0));
                existingOrder.ShippingAddress = order.ShippingAddress;
                existingOrder.TrackingNumber = Guid.NewGuid().ToString();
                existingOrder.PaymentMethod = order.PaymentMethod;
                existingOrder.Product.Quantity -= order.Quantity;

                _context.Update(existingOrder.Product);
                _context.Update(existingOrder);
            }
            else
            {
                order.UserId = user.Id; 
                order.ApplicationUser = user; 
                order.TotalPrice = order.Quantity * (product.DiscountedPrice + (product.DeliveryCharge ?? 0));
                product.Quantity -= order.Quantity;
                order.TrackingNumber = Guid.NewGuid().ToString();

                _context.Add(order);
                _context.Update(product);
            }

            var cartItem = await _context.Carts
       .FirstOrDefaultAsync(c => c.UserId == user.Id && c.ProductId == order.ProductId);
            if (cartItem != null)
            {
                _context.Carts.Remove(cartItem);
            }

            await _context.SaveChangesAsync();

            return RedirectToAction("Index", "Home", new { orderId = order.OrderId });
        }



        public async Task<IActionResult> ProductOrderDetails(int orderId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var order = await _context.Orders
     .Include(o => o.ApplicationUser)
     .Include(o => o.Product)
     .ThenInclude(p => p.Category)
     .FirstOrDefaultAsync(o => o.OrderId == orderId && o.UserId == userId);

            if (order == null || order.ApplicationUser == null)
            {
                // Log issue for debugging
                _logger.LogWarning("Order or ApplicationUser not found for OrderId: {OrderId}", orderId);
            }


            if (order == null)
            {
                return NotFound("Order not found or user is not authorized to view this order");
            }

            ViewBag.DeliveryCharge = order.Product.DeliveryCharge;


            return View(order);
        }


        [HttpGet]
        public async Task<IActionResult> OrderFromCart(int productId, int quantity)
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

            // Create an order object with the provided quantity
            var order = new Order
            {
                UserId = userId,
                ProductId = productId,
                OrderDate = DateTime.UtcNow,
                Quantity = quantity, 
                TotalPrice = quantity * (product.DiscountedPrice + (product.DeliveryCharge ?? 0)),
                Status = "Pending",
                ShippingAddress = $"{user.Address}, {user.City}, {user.State}, {user.PinCode}",
                TrackingNumber = Guid.NewGuid().ToString(),
                PaymentStatus = "Unpaid",
                PaymentMethod = "CashOnDelivery",
                Product = product,
                ApplicationUser = user
            };

            return View("OrderDetails", order); // Reuse the existing OrderDetails view
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
            var orderItem = await _context.Orders.Include(p => p.Product).FirstOrDefaultAsync(o => o.OrderId == orderId);

            if (orderItem == null)
            {
                return NotFound();
            }

            if (orderItem.Product != null)
            {
                orderItem.Product.Quantity += orderItem.Quantity;
                _context.Products.Update(orderItem.Product);
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
