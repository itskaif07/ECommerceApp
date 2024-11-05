using ECommerceApp.Data;
using ECommerceApp.Models;
using ECommerceApp.ViewModel;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;


namespace ECommerceApp.Controllers
{
    public class ProductsController : Controller
    {
        public readonly ApplicationDbContext _context;
        private readonly ILogger<ProductsController> _logger;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly UserManager<ApplicationUser> _userManager;

        public ProductsController(ApplicationDbContext context, ILogger<ProductsController> logger, IWebHostEnvironment webHostEnvironment, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _logger = logger;
            _webHostEnvironment = webHostEnvironment;
            _userManager = userManager;
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

        public async Task<IActionResult> ProductEdit(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }

            ViewBag.Categories = new SelectList(_context.Categories, "Id", "Name", product.CategoryId);
            return View(product);
        }


        // POST: ProductEdit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ProductEdit(Product model)
        {
            _logger.LogInformation("Editing product with ID: {Id}", model.Id);
            _logger.LogInformation("Received model - Name: {Name}, Price: {Price}, CategoryId: {CategoryId}", model.Name, model.Price, model.CategoryId);


            if (!ModelState.IsValid)
            {
                // Log all model state errors
                foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
                {
                    _logger.LogWarning("Model state error for product ID: {Id}, Error: {ErrorMessage}", model.Id, error.ErrorMessage);
                }

                ViewBag.Categories = new SelectList(_context.Categories, "Id", "Name", model.CategoryId);
                return View(model);
            }

            if (!ModelState.IsValid)
            {
                // Log model state errors
                foreach (var state in ModelState)
                {
                    foreach (var error in state.Value.Errors)
                    {
                        _logger.LogWarning("Model state error for {Key}: {ErrorMessage}", state.Key, error.ErrorMessage);
                    }
                }


                var product = await _context.Products.FindAsync(model.Id);
                if (product == null)
                {
                    _logger.LogWarning("Product with ID: {Id} not found for editing", model.Id);
                    return NotFound();
                }

                // Update properties
                product.Name = model.Name;
                product.Price = model.Price;
                product.Description = model.Description;
                product.Discount = model.Discount;
                product.CategoryId = model.CategoryId;

                // Image handling
                if (model.ImageFile != null)
                {
                    _logger.LogInformation("Uploading new image for product ID: {Id}", model.Id);
                    var uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "images");
                    var uniqueFileName = Guid.NewGuid().ToString() + "_" + model.ImageFile.FileName;
                    var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await model.ImageFile.CopyToAsync(fileStream);
                    }

                    // Delete the old image if a new one is uploaded
                    if (!string.IsNullOrEmpty(product.ImageUrl))
                    {
                        var oldFilePath = Path.Combine(_webHostEnvironment.WebRootPath, product.ImageUrl.TrimStart('/'));
                        if (System.IO.File.Exists(oldFilePath))
                        {
                            _logger.LogInformation("Deleting old image at path: {OldFilePath}", oldFilePath);
                            System.IO.File.Delete(oldFilePath);
                        }
                    }
                    product.ImageUrl = "/images/" + uniqueFileName;
                }

                await _context.SaveChangesAsync();
                _logger.LogInformation("Product ID: {Id} updated successfully", model.Id);
                return RedirectToAction("Details", new { id = product.Id });
            }

            ViewBag.Categories = new SelectList(await _context.Categories.ToListAsync(), "Id", "Name", model.CategoryId);
            return View(model);

        }

        public async Task<IActionResult> ProductDelete(int id)
        {
            var product = await _context.Products.FirstOrDefaultAsync(p => p.Id == id);

            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        [HttpPost]
        public async Task<IActionResult> ConfirmProductDelete(int id)
        {
            var product = await _context.Products.FirstOrDefaultAsync(p => p.Id == id);

            if (product == null)
            {
                return NotFound();
            }

            _context.Products.Remove(product);
            await _context.SaveChangesAsync();

           
            return RedirectToAction("Index", "Home"); 
        }


    }

}


