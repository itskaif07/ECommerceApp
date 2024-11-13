using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ECommerceApp.Models
{
    public class Cart
    {
        public int Id { get; set; }

        [Required]
        public string UserId { get; set; }

        [Required]
        public int ProductId { get; set; }

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Quantity must be at least 1.")]
        public int Quantity { get; set; } = 1;

        [Required]
        public DateTime DateAdded { get; set; } = DateTime.UtcNow;
         
        public Product product { get; set; }

       public ApplicationUser applicationUser { get; set; }

        public int? OrderId { get; set; }
        public Order Order { get; set; }

    }
}
