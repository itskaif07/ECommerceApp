﻿using ECommerceApp.Models;

namespace ECommerceApp.ViewModels
{
    public class ProductUserViewModel
    {
        public Product Product { get; set; }
        public ApplicationUser ApplicationUser { get; set; }

        public bool IsInWishlist { get; set; }
    }
}
