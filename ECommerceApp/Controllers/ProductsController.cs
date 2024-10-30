using ECommerceApp.Data;
using ECommerceApp.Models;
using ECommerceApp.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ECommerceApp.Controllers
{
    public class ProductsController : Controller
    {
        public readonly ApplicationDbContext _context;

        public ProductsController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> ProductsIndex(int categoryId)
        {
            if (!await _context.Categories.AnyAsync(c => c.Id == categoryId))
            {
                return NotFound("Category not found.");
            }

            var categories = await _context.Categories.ToListAsync();
            var products = await _context.Products.Include(p => p.Category)
                                                  .Where(p => p.CategoryId == categoryId)
                                                  .ToListAsync();

            if (categories == null || products == null)
            {
                return NotFound("Categories or Products could not be loaded.");
            }

            var viewModel = new HomeViewModel
            {
                Categories = categories,
                Products = products
            };


            return View("~/Views/Products/ProductsIndex.cshtml", viewModel);
        }

        public async Task<IActionResult> ProductDetails(int id)
        {
          
            var product = await _context.Products.Include(p => p.Category).FirstOrDefaultAsync(c => c.Id == id);

            if (product == null)
            {
                return NotFound("Product not found."); 
            }

           
            return View("~/Views/Products/ProductDetails.cshtml", product); 
        }
    }
}
