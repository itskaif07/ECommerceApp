using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ECommerceApp.Models
{
    public class Review
    {

        public int Id { get; set; }
        public int ProductId { get; set; }

        [ForeignKey("User")]
        public string UserId { get; set; }

        [Range(1, 5, ErrorMessage = "Rating should be between 1 and 5.")]
        [Required]
        public int Rating { get; set; }

        [StringLength(50, ErrorMessage = "Review name cannot exceed 50 characters")]
        [Required]
        [DisplayName("Review Name")]
        public string ReviewName { get; set; }

        [MaxLength(1000)]
        [DisplayName("Review Description")]
        public string? Comment { get; set; }

        public string? ImageUrl { get; set; }

        [NotMapped]
        public IFormFile? ImageFile { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        [ValidateNever]
        public Product Product { get; set; }

        [ValidateNever]
        public ApplicationUser User { get; set; } 


    }
}
