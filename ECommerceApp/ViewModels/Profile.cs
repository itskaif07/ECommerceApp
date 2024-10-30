using System.ComponentModel.DataAnnotations;

namespace ECommerceApp.ViewModels
{
    public class Profile
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Name is required.")]
        public string Name { get; set; }

        public string? UserName { get; set; } 

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email address.")]
        public string Email { get; set; } 

        [Phone(ErrorMessage = "Invalid phone number.")]
        [StringLength(10, ErrorMessage = "Phone number cannot exceed 10 characters")]
        public string? PhoneNumber { get; set; } 

        public string? Address { get; set; }

        public string? City { get; set; }

        public string? State { get; set; }

        [Required(ErrorMessage = "Pincode is required.")]
        [StringLength(10, MinimumLength = 5, ErrorMessage = "Pincode must be between 5 and 10 characters.")]
        public string Pincode { get; set; }
    }
}
