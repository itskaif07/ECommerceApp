using System.ComponentModel.DataAnnotations;

namespace ECommerceApp.Models
{
    public class Category
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Category Name is required.")]
        [StringLength(100)]
        public string Name { get; set; }

        public ICollection<Product> Products { get; set; } = new List<Product>();
    }
}
