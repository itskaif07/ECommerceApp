using ECommerceApp.Data;
using ECommerceApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ECommerceApp.Controllers
{
    public class CategoriesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CategoriesController(ApplicationDbContext context)
        {
            _context = context;
        }
        public IActionResult CategoryIndex()
        {
            var categories = _context.Categories.ToList();
            return View(categories);
        }

        public IActionResult CategoryAdd()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CategoryAdd(Category category)
        {
            

            if (ModelState.IsValid)
            {
                
                try
                {
                    await _context.Categories.AddAsync(category);
                    await _context.SaveChangesAsync();

                    return RedirectToAction("Index", "Home");
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
            else
            {
                foreach (var modelState in ModelState)
                {
                    foreach (var error in modelState.Value.Errors)
                    {
                        Console.WriteLine("Property: {Property}, Error: {ErrorMessage}", modelState.Key, error.ErrorMessage);
                    }
                }
            }

            return View(category); 
        }

        public async Task<IActionResult> DeleteCategoryList()
        {
            var categories = await _context.Categories.ToListAsync();
            return View(categories);
        }

        public async Task<IActionResult> CategoryDelete(int id)
        {
            var category = await _context.Categories.FindAsync(id);

            if (category == null)
            {
                return NotFound();
            }

            return View(category);
        }

        [HttpPost]
        public async Task<IActionResult> ConfirmCategoryDelete(int id)
        {
            var category = await _context.Categories.FindAsync(id);

            if (category == null)
            {
                return NotFound();
            }

            var products = await _context.Products.Where(p => p.CategoryId == id).ToListAsync();

            if (products.Any())
            {
                _context.Products.RemoveRange(products);
            }

            _context.Categories.Remove(category);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index", "Home");
        }


    }
}
