using ECommerceApp.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ECommerceApp.ViewModels;

namespace ECommerceApp.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<Product> Products { get; set; }

        public DbSet<Category> Categories { get; set; }

        public DbSet<Wishlist> Wishlists { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Product>()
                .Property(p => p.Price)
                .HasColumnType("decimal(18, 2)");

            modelBuilder.Entity<ApplicationUser>().
                HasIndex(u => u.Email)
                .IsUnique();

            modelBuilder.Entity<ApplicationUser>()
       .HasIndex(u => u.PhoneNumber)
       .IsUnique();

            modelBuilder.Entity<Category>().HasData(
               new Category { Id = 1, Name = "Electronics" },
               new Category { Id = 2, Name = "Books" },
               new Category { Id = 3, Name = "Clothing" },
               new Category { Id = 4, Name = "Toys" },
               new Category { Id = 5, Name = "Shoes" },
               new Category { Id = 6, Name = "Foods" },
               new Category { Id = 7, Name = "Beverages" },
               new Category { Id = 8, Name = "Movies" }
           );

            modelBuilder.Entity<Product>().HasData(


new Product
{
    Id = 1,
    Name = "Samsung Galaxy S21",
    Price = 79999,
    Description = "The Samsung Galaxy S21 features a stunning 6.2-inch Dynamic AMOLED display and is powered by the Exynos 2100 processor. With a triple camera setup and 5G connectivity, it provides high-quality photos and seamless streaming. The sleek design and long battery life make it perfect for daily use.",
    ImageUrl = "https://rajshahitech.com/wp-content/uploads/2023/12/2101-60481.jpg",
    CategoryId = 1,
    Discount = 10,
    Quantity = 5,
},

new Product
{
    Id = 2,
    Name = "To Kill a Mockingbird",
    Price = 599,
    Description = "A novel that explores the serious issues of rape and racial inequality through the eyes of a child in the 1930s South. It emphasizes the importance of moral integrity and empathy.",
    ImageUrl = "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcQnQ472uYK5UOemhgGD7wk3xpWJr4oDbsuY-A&s",
    CategoryId = 2,
    Discount = 5,
    Quantity = 2,

},

new Product
{
    Id = 3,
    Name = "Levi's Denim Jacket",
    Price = 4999,
    Description = "This classic Levi's denim jacket is a timeless piece that can be worn year-round. Made with durable denim, it features a button-up front and chest pockets for a stylish look. Perfect for layering, it adds an edge to any outfit.",
    ImageUrl = "https://dimg.dillards.com/is/image/DillardsZoom/main/levis-type-iii-sherpa-trucker-jacket/05120005_zi_mustard_wash.jpg",
    CategoryId = 3,
    Discount = 20,
    Quantity = 10,

},

new Product
{
    Id = 4,
    Name = "Nike Air Max 270",
    Price = 10999,
    Description = "The Nike Air Max 270 is designed for comfort and style. Featuring a large Air unit in the heel for responsive cushioning, these shoes provide all-day comfort. The sleek design is perfect for both workouts and casual wear.",
    ImageUrl = "https://rukminim2.flixcart.com/image/850/1000/xif0q/shoe/b/f/a/-original-imagx9kwcjzzjkej.jpeg?q=90&crop=false",
    CategoryId = 5,
    Discount = 15,
    Quantity = 14,

},

new Product
{
    Id = 5,
    Name = "Apple MacBook Air M1",
    Price = 99999,
    Description = "The MacBook Air with Apple M1 chip offers breakthrough performance with an 8-core CPU and up to 18 hours of battery life. Its lightweight design and Retina display make it a perfect choice for professionals on the go.",
    ImageUrl = "https://cdn.mos.cms.futurecdn.net/gPvyaz76tASn87RCGuSdDc-1200-80.jpg",
    CategoryId = 1,
    Discount = 10,
    Quantity = 6,

},

new Product
{
    Id = 6,
    Name = "Remote-Control Off-Road Monster Truck",
    Price = 2999,
    Description = "Experience thrilling off-road adventures with this durable, high-speed remote-control monster truck. Built to handle rough terrains, it's perfect for kids and adults who love outdoor fun.",
    ImageUrl = "https://rukminim2.flixcart.com/image/850/1000/xif0q/remote-control-toy/v/2/g/rock-crawler-rc-monster-truck-4wd-off-road-vehicle-multicolor-3-original-imafvhr45wdh2pry.jpeg?q=90&crop=false",
    CategoryId = 4,
    Discount = 15,
    Quantity = 10,

},


new Product
{
    Id = 7,
    Name = "The Godfather (DVD)",
    Price = 999,
    Description = "An iconic film about the powerful Italian-American crime family of Don Vito Corleone, exploring themes of power, loyalty, and family.",
    ImageUrl = "https://m.media-amazon.com/images/M/MV5BYTJkNGQyZDgtZDQ0NC00MDM0LWEzZWQtYzUzZDEwMDljZWNjXkEyXkFqcGc@._V1_.jpg",
    CategoryId = 8,
    Discount = 5,
    Quantity = 30,

},

new Product
{
    Id = 8,
    Name = "French Fries",
    Price = 299,
    Description = "Delicious and crispy French fries, perfect as a side or snack. Made with fresh potatoes and cooked to golden perfection, they pair well with any meal.",
    ImageUrl = "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcTCtQU2Pf3U32qKKJLQB0tY83xthaYEYTatRw&s",
    CategoryId = 6,
    Discount = 5,
    Quantity = 25,

},

new Product
{
    Id = 9,
    Name = "Pepsi",
    Price = 30,
    Description = "Pepsi is a refreshing soft drink that provides a burst of flavor. Perfect for parties or as a quick refreshment on a hot day.",
    ImageUrl = "https://www.jiomart.com/images/product/original/491208775/pepsi-750-ml-product-images-o491208775-p491208775-0-202203170727.jpg?im=Resize=(1000,1000)",
    CategoryId = 7,
    Discount = 0,
    Quantity = 40,

},

new Product
{
    Id = 10,
    Name = "Fitbit Charge 4",
    Price = 14999,
    Description = "The Fitbit Charge 4 is a fitness tracker that helps you monitor your activity levels, heart rate, and sleep patterns. With built-in GPS and long battery life, it’s perfect for fitness enthusiasts.",
    ImageUrl = "https://cdn.mos.cms.futurecdn.net/LaBALQRveyNDQjrac9HjC9.jpg",
    CategoryId = 1,
    Discount = 10,
    Quantity = 14,

},

new Product
{
    Id = 11,
    Name = "Acer Aspire 5 Slim Laptop",
    Price = 49999,
    Description = "The Acer Aspire 5 Slim Laptop is designed for students and professionals who need reliable performance in a lightweight build. It features a 15.6-inch Full HD IPS display for vibrant visuals and is powered by an Intel Core i5 processor, coupled with 8GB RAM and a 512GB SSD, delivering smooth multitasking and fast boot times. With up to 8 hours of battery life, it’s ideal for working on the go. It also includes a backlit keyboard, built-in webcam, and an advanced cooling system to keep it running efficiently even under heavy workloads.",
    ImageUrl = "https://m.media-amazon.com/images/I/71howy7q+HL.jpg",
    CategoryId = 1,
    Discount = 5,
    Quantity = 5,

},

new Product
{
    Id = 12,
    Name = "Samsung Galaxy Tab S7",
    Price = 61999,
    Description = "The Samsung Galaxy Tab S7 is a powerful tablet featuring a stunning 11-inch display and the latest Snapdragon processor. Ideal for both entertainment and productivity.",
    ImageUrl = "https://5.imimg.com/data5/SELLER/Default/2020/10/XD/SN/JS/51517768/samsung-tab-s7-27-81-cm-11-inch-6-gb-128-gb-wifi-mystic-black--500x500.jpg",
    CategoryId = 1,
    Discount = 15,
    Quantity = 8,

},

new Product
{
    Id = 13,
    Name = "Puma Running Shoes",
    Price = 5999,
    Description = "Puma Running Shoes are designed for comfort and performance. With a lightweight design and excellent grip, they are perfect for runners of all levels.",
    ImageUrl = "https://za.puma.com/dw/image/v2/BDSF_PRD/on/demandware.static/-/Sites-ZA-Library/default/dwb6844bf2/Fast-R2.jpg?q=80&sw=720",
    CategoryId = 5,
    Discount = 10,
    Quantity = 10,

},

new Product
{
    Id = 14,
    Name = "Harry Potter and the Sorcerer's Stone",
    Price = 499,
    Description = "The first book in the Harry Potter series introduces readers to a young boy who discovers he is a wizard and attends Hogwarts School of Witchcraft and Wizardry, embarking on magical adventures.",
    ImageUrl = "https://images-na.ssl-images-amazon.com/images/I/81iqZ2HHD-L.jpg",
    CategoryId = 2,
    Discount = 15,
    Quantity = 4,

},

new Product
{
    Id = 15,
    Name = "Asus ROG Zephyrus G14",
    Price = 149999,
    Description = "The Asus ROG Zephyrus G14 is a gaming laptop that offers exceptional performance with its Ryzen 9 processor and high-refresh-rate display. Perfect for gamers who demand power and portability.",
    ImageUrl = "https://m.media-amazon.com/images/I/71qbOU164lL._AC_UF1000,1000_QL80_.jpg",
    CategoryId = 1,
    Discount = 8,
    Quantity = 6,

},

new Product
{
    Id = 16,
    Name = "Coca Cola",
    Price = 15,
    Description = "Coca Cola is a classic soft drink that delivers a unique taste. Enjoy it chilled with friends or during family gatherings.",
    ImageUrl = "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcT3M-xR4PTf-oldWP8s2UWBCDxtZatNqXs1NA&s",
    CategoryId = 7,
    Discount = 0,
    Quantity = 20,

},

new Product
{
    Id = 17,
    Name = "Nike Sportswear Hoodie",
    Price = 3999,
    Description = "This Nike sportswear hoodie is perfect for workouts or casual wear. Made with soft fabric, it keeps you warm while providing style.",
    ImageUrl = "https://images-cdn.ubuy.co.in/6699915c9d25aa62af63d3d6-nike-sportswear-boys-39-club-pullover.jpg",
    CategoryId = 3,
    Discount = 15,
    Quantity = 0,

},

new Product
{
    Id = 18,
    Name = "Nerf N-Strike Elite Disruptor",
    Price = 1499,
    Description = "A Nerf blaster that fires darts up to 90 feet. It features a rotating drum that holds up to 6 darts, providing quick reloads and rapid firing.",
    ImageUrl = "https://m.media-amazon.com/images/I/51LfZ+hdD5L.jpg",
    CategoryId = 4,
    Discount = 10,
    Quantity = 0,

},

new Product
{
    Id = 19,
    Name = "Hershey's Chocolate",
    Price = 150,
    Description = "Hershey's chocolate is a beloved treat that is perfect for any occasion. Enjoy its rich and creamy taste in various flavors.",
    ImageUrl = "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcQmatVcE5DIhAEjMm69GwVJu0Ge_UqwKVkvSQ&s",
    CategoryId = 6,
    Discount = 8,
},

new Product
{
    Id = 20,
    Name = "Inception",
    Price = 999,
    Description = "Inception is a sci-fi thriller directed by Christopher Nolan. The story follows a skilled thief who steals secrets from deep within the subconscious during the dream state. This mind-bending film combines stunning visuals with a complex narrative, exploring the themes of dreams and reality. With an all-star cast and a gripping score, Inception is a must-watch for any film enthusiast.",
    ImageUrl = "https://m.media-amazon.com/images/I/912AErFSBHL._AC_UF894,1000_QL80_.jpg",
    CategoryId = 8,
    Discount = 15,
    Quantity = 6,

}

   );
        }
    }
};