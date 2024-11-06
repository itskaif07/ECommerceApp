﻿using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ECommerceApp.Models
{
    public class Product
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Product Name is required.")]
        [StringLength(100)]
        public string Name { get; set; }

        public string? Description { get; set; }

        [Range(0.01, double.MaxValue, ErrorMessage = "Price must be a positive number.")]
        [Required(ErrorMessage = "Price is required.")]
        public decimal Price { get; set; }

        public string? ImageUrl { get; set; }

        [NotMapped]
        public IFormFile? ImageFile { get; set; }

        [NotMapped]
        public string? WebUrl { get; set; }

        [Range(0, 100, ErrorMessage = "Discount must be between 0 and 100.")]
        public int Discount { get; set; } = 0;

        [NotMapped]
        public decimal DiscountedPrice
        {
            get
            {
                if (Discount > 0)
                {
                    return Price - (Price * (Discount / 100m));
                }
                return Price;
            }
        }

        [Range(0, int.MaxValue, ErrorMessage = "Quantity cannot be negative.")]
        public int Quantity { get; set; } = 0;


        [ForeignKey("Category")]
        [Display(Name = "Category")]
        [Required(ErrorMessage = "Category is Required")]

        public int CategoryId { get; set; }

        public Category Category { get; set; }


    }
}