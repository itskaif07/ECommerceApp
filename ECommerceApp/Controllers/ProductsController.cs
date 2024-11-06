using ECommerceApp.Data;
using ECommerceApp.Models;
using ECommerceApp.ViewModel;
using ECommerceApp.ViewModels;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Security.Claims;


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
            var product = await _context.Products
                .Include(p => p.Category)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (product == null)
            {
                return NotFound("Product not found.");
            }


            var user = await _userManager.GetUserAsync(User);

            if (user == null)
            {
                return Unauthorized("User not found");
            }

            bool isInWishList = await _context.Wishlists.AnyAsync(w => w.ProductId == id && w.UserId == user.Id);

            var viewModel = new ProductUserViewModel
            {
                Product = product,
                ApplicationUser = user,
                IsInWishlist = isInWishList
            };

            return View("~/Views/Products/ProductDetails.cshtml", viewModel);
        }


        [HttpGet]
        public IActionResult ProductAdd(int productId)
        {
            ViewBag.Categories = new SelectList(_context.Categories, "Id", "Name");

            var product = _context.Products.FirstOrDefault(p => p.Id == productId);


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
            else if (!string.IsNullOrEmpty(product.WebUrl))
            {
                product.ImageUrl = product.WebUrl;
            }


            _context.Add(product);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index", "Home");
        }




        [HttpGet]
        public async Task<IActionResult> ProductEdit(int productId)
        {
            var product = await _context.Products.FindAsync(productId);
            if (product == null)
            {
                return NotFound();
            }

            ViewBag.Categories = new SelectList(_context.Categories, "Id", "Name", product.CategoryId);

            return View(product);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ProductEdit(Product model)
        {

            if (!ModelState.IsValid)
            {

                ViewBag.Categories = new SelectList(_context.Categories, "Id", "Name", model.CategoryId);
                return View(model);
            }

            if (model.ImageFile != null)
            {
                var filePath = Path.Combine("wwwroot/images", model.ImageFile.FileName);
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await model.ImageFile.CopyToAsync(stream);
                }

                model.ImageUrl = "/images/" + model.ImageFile.FileName;
            }
            else if (!string.IsNullOrEmpty(model.WebUrl))
            {
                model.ImageUrl = model.WebUrl;
            }

            var productToUpdate = await _context.Products.FindAsync(model.Id);
            if (productToUpdate == null)
            {
                return NotFound();
            }

            productToUpdate.Name = model.Name;
            productToUpdate.Price = model.Price;
            productToUpdate.Description = model.Description;
            productToUpdate.Discount = model.Discount;
            productToUpdate.Quantity = model.Quantity;
            productToUpdate.CategoryId = model.CategoryId;
            productToUpdate.ImageUrl = model.ImageUrl;

            _context.Products.Update(productToUpdate);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index", "Home");
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


