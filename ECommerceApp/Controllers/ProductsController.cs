using ECommerceApp.Data;
using ECommerceApp.Models;
using ECommerceApp.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace ECommerceApp.Controllers
{
    public class ProductsController : Controller
    {
        public readonly ApplicationDbContext _context;

        public ProductsController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> AllProducts()
        {
            var products = await _context.Products.ToListAsync();
            return View(products);
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

        [HttpGet]
        public IActionResult ProductAdd()
        {
            ViewBag.Categories = new SelectList(_context.Categories, "Id", "Name");

            return View("~/Views/Products/ProductAdd.cshtml");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ProductAdd(Product product)
        {

            if (!ModelState.IsValid)
            {

                foreach (var state in ModelState)
                {
                    Console.WriteLine($"Key: {state.Key} - Errors: {string.Join(", ", state.Value.Errors.Select(e => e.ErrorMessage))}");
                }
            }

            if (product.ImageFile != null)
            {
                var filePath = Path.Combine("wwwroot/images", product.ImageFile.FileName);
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await product.ImageFile.CopyToAsync(stream);
                }
                product.ImageUrl = "/images/" + product.ImageFile.FileName;
            }
            else if(!string.IsNullOrEmpty(product.WebUrl))
            {
                product.ImageUrl = product.WebUrl;
            }

       
            _context.Add(product);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index", "Home");
        }


    }
}
