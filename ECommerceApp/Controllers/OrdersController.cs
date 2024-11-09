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

        public async Task<IActionResult> OrderIndex()
        {
            var orders = _context.Orders.Include(o => o.Product);
            return View(await orders.ToListAsync());
        }


        [HttpPost]
        public async Task<IActionResult> DeleteAll()
        {
            var orderItem = await _context.Orders.Include(p => p.Product).ToListAsync();

            _context.Orders.RemoveRange(orderItem);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index", "Home");
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

            if (order == null)
            {
                order = new Order
                {
                    UserId = userId,
                    ProductId = productId,
                    OrderDate = DateTime.UtcNow,
                    TotalPrice = product.DiscountedPrice,
                    Status = "Pending",
                    ShippingAddress = $"{user?.Address}, {user?.City}, {user?.State}, {user?.PinCode}",
                    TrackingNumber = Guid.NewGuid().ToString(),
                    PaymentStatus = "Unpaid",
                    PaymentMethod = "CashOnDelivery" 
                };

                _context.Orders.Add(order);
                await _context.SaveChangesAsync();
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

            var existingOrder = await _context.Orders
                                  .Include(o => o.Product)  
                                  .FirstOrDefaultAsync(o => o.UserId == order.UserId && o.ProductId == order.ProductId);

            if (existingOrder != null)
            {
                if (existingOrder.Product != null)
                {
                    existingOrder.Quantity = order.Quantity;  
                    existingOrder.TotalPrice = order.Quantity * existingOrder.Product.DiscountedPrice; 
                    existingOrder.ShippingAddress = order.ShippingAddress;
                    existingOrder.TrackingNumber = Guid.NewGuid().ToString(); 
                    existingOrder.PaymentMethod = order.PaymentMethod;

                    _context.Update(existingOrder); 
                }
                else
                {
                    ModelState.AddModelError("", "Product not found for this order.");
                    return View(order);
                }
            }
            else
            {
                order.TotalPrice = order.Quantity * order.Product.DiscountedPrice;  
                order.TrackingNumber = Guid.NewGuid().ToString(); 

                _context.Add(order); 
            }

            await _context.SaveChangesAsync();  

            return RedirectToAction("Index", "Home", new { orderId = order.OrderId });  


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
