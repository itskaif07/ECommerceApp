using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace ECommerceApp.ViewModels.Authentication
{
    public class SignUp
    {
        [Required(ErrorMessage = "Name is Required")]
        [StringLength(100, ErrorMessage = "Name cannot exceed 100 characters")]
        public string Name { get; set; }

        [Display(Name = "Username")]
        public string UserName { get; set; }

        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Confirm Password")]
        [Compare("Password", ErrorMessage = "Passwords do not match.")]
        public string ConfirmPassword { get; set; }

        [Phone(ErrorMessage = "Invalid phone number format")]
        [StringLength(10, ErrorMessage = "Phone number cannot exceed 10 characters")]
        [DisplayName("Phone Number")]
        public string? PhoneNumber { get; set; }

        public string? Address { get; set; }

        [StringLength(100, ErrorMessage = "City cannot exceed 100 characters")]
        public string? City { get; set; }

        [StringLength(100, ErrorMessage = "State cannot exceed 100 characters")]
        public string? State { get; set; }

        [DisplayName("Pin Code")]
        [Required(ErrorMessage = "Pin Code is Required")]
        [RegularExpression(@"^\d{6}$", ErrorMessage = "Invalid pin code. It must be 6 digits.")]
        public string PinCode { get; set; }
    }
}
