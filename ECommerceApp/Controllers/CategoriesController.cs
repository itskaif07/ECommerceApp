using ECommerceApp.Data;
using ECommerceApp.Models;
using Microsoft.AspNetCore.Identity.UI.V4.Pages.Internal;
using Microsoft.AspNetCore.Mvc;

namespace ECommerceApp.Controllers
{
    public class CategoriesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<CategoriesController> _logger;

        public CategoriesController(ApplicationDbContext context, ILogger<CategoriesController> logger)
        {
            _context = context;
            _logger = logger;
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
                    _logger.LogError(ex, "Error saving category to database.");
                }
            }
            else
            {
                foreach (var modelState in ModelState)
                {
                    foreach (var error in modelState.Value.Errors)
                    {
                        _logger.LogWarning("Property: {Property}, Error: {ErrorMessage}", modelState.Key, error.ErrorMessage);
                    }
                }
            }

            return View(category); 
        }


    }
}
