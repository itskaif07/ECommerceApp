using ECommerceApp.Models;

namespace ECommerceApp.ViewModel
{
    public class HomeViewModel
    {
        public IEnumerable<Product> Products { get; set; }

        public IEnumerable<Category> Categories { get; set; }

        public IEnumerable<Cart> Carts { get; set; }
    }
}
