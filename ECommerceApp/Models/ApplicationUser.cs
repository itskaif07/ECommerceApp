using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace ECommerceApp.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string Name { get; set; }
        public string? Address { get; set; }
        public string? City { get; set; }
        public string? State { get; set; }

        [StringLength(10, MinimumLength = 5)]
        public string PinCode { get; set; }
    }
}
