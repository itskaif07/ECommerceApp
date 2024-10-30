using System.ComponentModel.DataAnnotations;

namespace ECommerceApp.ViewModels.Authentication
{
    public class LogIn
    {
        [Display(Name = "Username")]
        public string? UserName { get; set; }

        [Required]
        [Display(Name = "Email")]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        public bool RememberMe { get; set; }
    }
}
