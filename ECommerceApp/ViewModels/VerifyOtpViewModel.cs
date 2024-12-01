using System.ComponentModel.DataAnnotations;

namespace ECommerceApp.ViewModels
{
    public class VerifyOtpViewModel
    {
        [Required]
        public string UserId { get; set; }

        [Required]
        [StringLength(6, MinimumLength = 6, ErrorMessage = "OTP must be 6 digits.")]
        public string Otp { get; set; }
    }
}
