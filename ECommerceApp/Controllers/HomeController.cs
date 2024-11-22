﻿using ECommerceApp.Data;
using ECommerceApp.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using ECommerceApp.ViewModel;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace ECommerceApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
        }
        public async Task<IActionResult> Index(int? categoryId)
        {
            var products = categoryId == null
                ? await _context.Products.ToListAsync()
                : await _context.Products.Where(p => p.CategoryId == categoryId).ToListAsync();

            var categories = await _context.Categories.ToListAsync();
            var carts = await _context.Carts.ToListAsync();
            var orders = await _context.Orders.ToListAsync();

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier); 

            var userCartItems = await _context.Carts
                .Where(c => c.UserId == userId) 
                .ToListAsync();

            var userOrderItems = await _context.Orders
                .Where(o => o.UserId == userId)
                .ToListAsync();

            var totalCartQuantity = userCartItems.Sum(i => i.Quantity); 
            var totalOrderQuantity = userOrderItems.Sum(i => i.Quantity); 


            ViewData["CartQuantity"] = totalCartQuantity;
            ViewData["OrderQuantity"] = totalOrderQuantity;

            var model = new HomeViewModel
            {
                Products = products,
                Categories = categories,
                Carts = carts,
                Orders = orders,
            };

            return View(model);
        }


        public IActionResult Admin()
        {
            return View();
        }




        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
