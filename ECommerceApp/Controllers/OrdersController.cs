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
using ECommerceApp.Helpers;

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

        //Order List

        public async Task<IActionResult> OrderIndex()
        {
            var user = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var orders = await _context.Orders.Include(o => o.Product).Where(u => u.UserId == user).OrderByDescending(o => o.OrderDate).ToListAsync();

            ViewBag.TotalAmount = orders.Sum(o => o.TotalPrice);
            return View(orders);
        }

        //Add Order

        [HttpGet]
        public async Task<IActionResult> OrderDetails(int? orderId, int productId, int deliveryCharge, DateTime deliveryDate, int? quantity)
        {
            try
            {
                // Get the logged-in user's ID
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (userId == null)
                {
                    return RedirectToAction("Login", "Login");
                }

                // Fetch the product details
                var product = await _context.Products.FindAsync(productId);
                if (product == null)
                {
                    return NotFound("Product not found.");
                }

                // Fetch the logged-in user's details
                var user = await _userManager.GetUserAsync(User);
                if (user == null)
                {
                    return NotFound("User not found.");
                }

                // Check if an existing order matches the given parameters
                var order = await _context.Orders
                    .Include(o => o.Product)
                    .FirstOrDefaultAsync(o => o.UserId == userId && o.ProductId == productId && o.OrderId == orderId);

                // If no order exists, create a new order object (but do not save it to the database here)
                if (order == null)
                {
                    order = new Order
                    {
                        UserId = userId,
                        ProductId = productId,
                        OrderDate = DateTime.UtcNow,
                        TotalPrice = (product.DiscountedPrice * (quantity ?? 1)) + deliveryCharge,
                        Status = "Pending",
                        ShippingAddress = $"{user.Address}, {user.City}, {user.State}, {user.PinCode}",
                        TrackingNumber = Guid.NewGuid().ToString(),
                        PaymentStatus = "Unpaid",
                        PaymentMethod = "CashOnDelivery",
                        Quantity = quantity ?? 1,
                        DeliveryCharge = deliveryCharge,
                        DeliveryDate = deliveryDate,
                        Product = product,
                        ApplicationUser = user
                    };
                }

                // Return the OrderDetails view with the order object
                return View(order);
            }
            catch (Exception ex)
            {
                // Log the exception and return an error status
                Console.WriteLine($"Error: {ex.Message}");
                return StatusCode(500, "An error occurred. Please try again later.");
            }
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> OrderDetails(Order order)
        {
            if (order == null)
            {
                TempData["Notification"] = NotificationHelper.FormatNotification("Order object is null.", "error");
                return View(order);
            }

            var product = await _context.Products.FindAsync(order.ProductId);
            if (product == null)
            {
                TempData["Notification"] = NotificationHelper.FormatNotification("Product not found.", "error");
                return View(order);
            }

            // Ensure that Product is assigned to the Order
            order.Product = product;

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                TempData["Notification"] = NotificationHelper.FormatNotification("User not found.", "error");
                return View(order);
            }

  

            // Validate Minimum Price - If total price is too low, show error
            var minimumPrice = product.DiscountedPrice * order.Quantity;
            if (minimumPrice < 100)
            {
                TempData["Notification"] = NotificationHelper.FormatNotification(NotificationHelper.GetNotificationMessage("minimumprice").message, "error");
                return View(order); 
            }

            // Validate Stock - If quantity exceeds stock, show error
            if (order.Quantity > product.Quantity)
            {
                TempData["Notification"] = NotificationHelper.FormatNotification($"Only {product.Quantity} in stock! Please adjust your order.", "error");
                return View(order);
            }


            order.UserId = user.Id;
            order.ApplicationUser = user;
            order.OrderDate = DateTime.UtcNow;
            order.Status = "Pending";
            order.PaymentStatus = "Pending";
            order.TotalPrice = (order.Quantity * product.DiscountedPrice) + (order.DeliveryCharge ?? 0);
            order.TrackingNumber = Guid.NewGuid().ToString();

            
            if (order.Quantity <= product.Quantity) 
            {
                product.Quantity -= order.Quantity;
            }
            else
            {
                // If validation failed (this should not occur if handled earlier)
                TempData["Notification"] = NotificationHelper.FormatNotification("Insufficient stock to reduce.", "error");
                return View(order);
            }

            
            _context.Add(order);
            _context.Update(product);

            // Remove item from cart if present
            var cartItem = await _context.Carts
                .FirstOrDefaultAsync(c => c.UserId == user.Id && c.ProductId == order.ProductId);
            if (cartItem != null)
            {
                _context.Carts.Remove(cartItem);
            }

            await _context.SaveChangesAsync();

           
            TempData["Notification"] = NotificationHelper.FormatNotification(NotificationHelper.GetNotificationMessage("ordercomplete").message, "success");

           
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

            if (order == null)
            {
                return NotFound("Order not found or user is not authorized to view this order");
            }


            return View(order);
        }

        // Cart Order

        [HttpPost]
        public async Task<IActionResult> PlaceOrder(int DeliveryCharge, DateTime DeliveryDate)
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
                    TotalPrice = (cartItem.Quantity * cartItem.product.DiscountedPrice) + DeliveryCharge,
                    OrderDate = DateTime.UtcNow,
                    TrackingNumber = Guid.NewGuid().ToString(),
                    ShippingAddress = shippingAddress,
                    PaymentMethod = "CashOnDelivery",
                    DeliveryCharge = DeliveryCharge,
                    DeliveryDate = DeliveryDate,
                    ApplicationUser = user
                };

                _context.Orders.Add(order);
            }

            _context.Carts.RemoveRange(cartItems);

            await _context.SaveChangesAsync();

            string actionResult = "ordercomplete";

            var (message, notificationStyle) = NotificationHelper.GetNotificationMessage(actionResult);
            var formattedNotification = NotificationHelper.FormatNotification(message, notificationStyle);

            TempData["Notification"] = formattedNotification;

            return RedirectToAction("CartsIndex", "Carts");
        }


        [HttpGet]
        public async Task<IActionResult> OrderFromCart(int productId, int quantity, int deliveryCharge, DateTime deliveryDate)
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


            // Create an order object with the provided quantity and delivery details
            var order = new Order
            {
                UserId = userId,
                ProductId = productId,
                OrderDate = DateTime.UtcNow,
                Quantity = quantity,
                TotalPrice = quantity * (product.DiscountedPrice + deliveryCharge),
                Status = "Pending",
                ShippingAddress = $"{user.Address}, {user.City}, {user.State}, {user.PinCode}",
                TrackingNumber = Guid.NewGuid().ToString(),
                PaymentStatus = "Unpaid",
                PaymentMethod = "CashOnDelivery",
                Product = product,
                ApplicationUser = user,
                DeliveryCharge = deliveryCharge,
                DeliveryDate = deliveryDate,
            };

            return View("OrderDetails", order);
        }


        //Delete Order

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

            string actionResult = "orderdelete";

            var (message, notification) = NotificationHelper.GetNotificationMessage(actionResult);

            string formattedNotification = NotificationHelper.FormatNotification(message, notification);

            TempData["Notification"] = formattedNotification;

            return RedirectToAction("OrderIndex", "Orders");
        }


        [HttpPost]
        public async Task<IActionResult> DeleteAll()
        {
            var orderItem = await _context.Orders.Include(p => p.Product).ToListAsync();

            foreach (var order in orderItem)
            {
                var product = order.Product;

                if (product != null)
                {
                    product.Quantity += order.Quantity;
                    _context.Update(product);
                }
            }

            _context.Orders.RemoveRange(orderItem);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index", "Home");
        }

        private bool OrderExists(int id)
        {
            return _context.Orders.Any(e => e.OrderId == id);
        }
    }
}
