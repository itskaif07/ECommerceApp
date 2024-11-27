using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ECommerceApp.Data;
using ECommerceApp.Models;
using ECommerceApp.Helpers;
using Microsoft.CodeAnalysis;

namespace ECommerceApp.Controllers
{
    public class ReviewsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ReviewsController(ApplicationDbContext context)
        {
            _context = context;
        }


        // GET: Reviews/Create
        public IActionResult AddReview(string userId, int productId)
        {
            ViewBag.ProductId = productId;
            ViewBag.UserId = userId;


            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddReview([Bind("Id,ProductId,UserId,Rating,ReviewName,Comment,ImageUrl,ImageFile,CreatedAt")] Review review)
        {
            if (ModelState.IsValid)
            {
                // Save image file if uploaded
                if (review.ImageFile != null)
                {
                    var filePath = Path.Combine("wwwroot/images", review.ImageFile.FileName);
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await review.ImageFile.CopyToAsync(stream);
                    }
                    review.ImageUrl = "/images/" + review.ImageFile.FileName;
                }

                _context.Add(review);
                await _context.SaveChangesAsync();
                TempData["Notification"] = NotificationHelper.FormatNotification("Review Added", "success");
                return RedirectToAction("ProductDetails", "Products", new { id = review.ProductId });
            }

            // If validation fails, keep the uploaded image's URL
            if (!string.IsNullOrEmpty(review.ImageUrl))
            {
                ViewBag.ImageUrl = review.ImageUrl;
            }
            ViewBag.ProductId = review.ProductId;
            ViewBag.UserId = review.UserId;
            return View(review);
        }


        // GET: Reviews/Edit/5
        // GET: EditReview
        public async Task<IActionResult> EditReview(int? id) // Renamed from Edit to EditReview
        {
            if (id == null)
            {
                return NotFound();
            }

            var review = await _context.Reviews.FindAsync(id);
            if (review == null)
            {
                return NotFound("Review Not Found");
            }

            var productId = review.ProductId;

            ViewBag.ProductId = productId;

            return View(review);
        }

        // POST: EditReview
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditReview(int id, [Bind("Id,ProductId,UserId,Rating,ReviewName,Comment,ImageUrl,ImageFile,CreatedAt")] Review review)
        {
            Console.WriteLine($"Received Review ID: {review.Id}");
            Console.WriteLine($"Product ID: {review.ProductId}, User ID: {review.UserId}");
            Console.WriteLine($"Rating: {review.Rating}, Comment: {review.Comment}");

            if (id != review.Id)
            {
                Console.WriteLine("Review ID mismatch.");
                return NotFound("Review ID mismatch.");
            }

            if (!ModelState.IsValid)
            {
                Console.WriteLine("Model state is invalid:");
                foreach (var error in ModelState)
                {
                    Console.WriteLine($"Key: {error.Key}, Error: {string.Join(", ", error.Value.Errors.Select(e => e.ErrorMessage))}");
                }
                return View(review); // Return view with the model to see validation errors
            }


            if (ModelState.IsValid)
            {
                try
                {
                    if (review.ImageFile != null)
                    {
                        var filePath = Path.Combine("wwwroot/images", review.ImageFile.FileName);
                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            await review.ImageFile.CopyToAsync(stream);
                        }
                        Console.WriteLine("Review updated successfully.");


                        review.ImageUrl = "/images/" + review.ImageFile.FileName;
                    }

                    _context.Update(review);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    Console.WriteLine($"Concurrency error: {ex.Message}");
                    if (!ReviewExists(review.Id))
                    {
                        return NotFound("Review not found in the database.");
                    }
                    else
                    {
                        throw;
                    }
                }
                // Redirect to ProductDetails after editing
                return RedirectToAction("ProductDetails", "Products", new { id = review.ProductId });
            }

            return View(review);
        }

        // GET: Reviews/Delete/5
        public async Task<IActionResult> DeleteReview(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var review = await _context.Reviews
                .Include(r => r.Product)
                .Include(r => r.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (review == null)
            {
                return NotFound();
            }

            return View(review);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteReviewConfirmed(int id)
        {
            var review = await _context.Reviews.FindAsync(id);
            if (review != null)
            {
                _context.Reviews.Remove(review);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction("ProductDetails", "Products", new { id = review.ProductId });

        }

        private bool ReviewExists(int id)
        {
            return _context.Reviews.Any(e => e.Id == id);
        }
    }
}
