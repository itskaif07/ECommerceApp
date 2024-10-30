using ECommerceApp.Data;
using ECommerceApp.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using ECommerceApp.ViewModel;
using Microsoft.EntityFrameworkCore;

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

        public async Task<IActionResult> Index()
        {
            var products = await _context.Products.ToListAsync();
            var categories = await _context.Categories.ToListAsync();

            var model = new HomeViewModel
            {
                Products = products,
                Categories = categories
            };

            return View(model);
        }




        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
