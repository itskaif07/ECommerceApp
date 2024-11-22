﻿using System;
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
        public async Task<IActionResult> OrderDetails(int? orderId, int productId, int deliveryCharge, DateTime deliveryDate)
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




            if (order == null)
            {
                order = new Order
                {
                    UserId = userId,
                    ProductId = productId,
                    OrderDate = DateTime.UtcNow,
                    TotalPrice = product.DiscountedPrice + (order?.DeliveryCharge ?? 0),
                    Status = "Pending",
                    ShippingAddress = $"{user.Address}, {user.City}, {user.State}, {user.PinCode}",
                    TrackingNumber = Guid.NewGuid().ToString(),
                    PaymentStatus = "Unpaid",
                    PaymentMethod = "CashOnDelivery",
                    DeliveryCharge = deliveryCharge,
                    DeliveryDate = deliveryDate,
                    Product = product,
                    ApplicationUser = user
                };
            }


            return View(order);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
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
                existingOrder.TotalPrice = (order.Quantity * product.DiscountedPrice) + (order.DeliveryCharge ?? 0);
                existingOrder.ShippingAddress = order.ShippingAddress;
                existingOrder.TrackingNumber = Guid.NewGuid().ToString();
                existingOrder.PaymentMethod = order.PaymentMethod;
                existingOrder.Product.Quantity -= order.Quantity;
                existingOrder.DeliveryCharge = order.DeliveryCharge;
                existingOrder.DeliveryDate = order.DeliveryDate;

                _context.Update(existingOrder.Product);
                _context.Update(existingOrder);
            }
            else
            {
                order.UserId = user.Id;
                order.ApplicationUser = user;
                order.TotalPrice = (order.Quantity * product.DiscountedPrice) + (order.DeliveryCharge ?? 0);
                product.Quantity -= order.Quantity;
                order.TrackingNumber = Guid.NewGuid().ToString();

                _context.Add(order);
                _context.Update(product);
            }

            var availableQuantity = product.Quantity;

            if (order.Quantity > availableQuantity)
            {
                string actionResult1 = "ordercomplete";

                var (message1, notificationType1) = NotificationHelper.GetNotificationMessage(actionResult1);
                string formattedNotification1 = NotificationHelper.FormatNotification(message1, notificationType1);

                TempData["Notification"] = formattedNotification1;

                return View(order);
            }

            var MinimumPrice = product.DiscountedPrice * order.Quantity;
            if (MinimumPrice <= 100)
            {
                TempData["Error"] = "Order must be of at least ₹100.";
                return View(order);
            }

         

            var cartItem = await _context.Carts
       .FirstOrDefaultAsync(c => c.UserId == user.Id && c.ProductId == order.ProductId);
            if (cartItem != null)
            {
                _context.Carts.Remove(cartItem);
            }

            await _context.SaveChangesAsync();

            string actionResult = "ordercomplete";

            var (message, notificationStyle) = NotificationHelper.GetNotificationMessage(actionResult);
            var formattedNotification = NotificationHelper.FormatNotification(message, notificationStyle);

            TempData["Notification"] = formattedNotification;

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
