﻿// <auto-generated />
using System;
using ECommerceApp.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace ECommerceApp.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20241108132157_OrderModel")]
    partial class OrderModel
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.10")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("ECommerceApp.Models.ApplicationUser", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("AccessFailedCount")
                        .HasColumnType("int");

                    b.Property<string>("Address")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("City")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Email")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<bool>("EmailConfirmed")
                        .HasColumnType("bit");

                    b.Property<bool>("LockoutEnabled")
                        .HasColumnType("bit");

                    b.Property<DateTimeOffset?>("LockoutEnd")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("PasswordHash")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("nvarchar(450)");

                    b.Property<bool>("PhoneNumberConfirmed")
                        .HasColumnType("bit");

                    b.Property<string>("PinCode")
                        .IsRequired()
                        .HasMaxLength(10)
                        .HasColumnType("nvarchar(10)");

                    b.Property<string>("SecurityStamp")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("State")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("TwoFactorEnabled")
                        .HasColumnType("bit");

                    b.Property<string>("UserName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.HasKey("Id");

                    b.HasIndex("Email")
                        .IsUnique()
                        .HasFilter("[Email] IS NOT NULL");

                    b.HasIndex("NormalizedEmail")
                        .HasDatabaseName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasDatabaseName("UserNameIndex")
                        .HasFilter("[NormalizedUserName] IS NOT NULL");

                    b.HasIndex("PhoneNumber")
                        .IsUnique()
                        .HasFilter("[PhoneNumber] IS NOT NULL");

                    b.ToTable("AspNetUsers", (string)null);
                });

            modelBuilder.Entity("ECommerceApp.Models.Cart", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("DateAdded")
                        .HasColumnType("datetime2");

                    b.Property<int>("ProductId")
                        .HasColumnType("int");

                    b.Property<int>("Quantity")
                        .HasColumnType("int");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("applicationUserId")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("ProductId");

                    b.HasIndex("applicationUserId");

                    b.ToTable("Carts");
                });

            modelBuilder.Entity("ECommerceApp.Models.Category", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.HasKey("Id");

                    b.ToTable("Categories");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Name = "Electronics"
                        },
                        new
                        {
                            Id = 2,
                            Name = "Books"
                        },
                        new
                        {
                            Id = 3,
                            Name = "Clothing"
                        },
                        new
                        {
                            Id = 4,
                            Name = "Toys"
                        },
                        new
                        {
                            Id = 5,
                            Name = "Shoes"
                        },
                        new
                        {
                            Id = 6,
                            Name = "Foods"
                        },
                        new
                        {
                            Id = 7,
                            Name = "Beverages"
                        },
                        new
                        {
                            Id = 8,
                            Name = "Movies"
                        });
                });

            modelBuilder.Entity("ECommerceApp.Models.Order", b =>
                {
                    b.Property<int>("OrderId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("OrderId"));

                    b.Property<string>("ApplicationUserId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<DateTime>("OrderDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("PaymentMethod")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PaymentStatus")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("ProductId")
                        .HasColumnType("int");

                    b.Property<int>("Quantity")
                        .HasColumnType("int");

                    b.Property<string>("ShippingAddress")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Status")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<decimal>("TotalPrice")
                        .HasColumnType("decimal(18,2)");

                    b.Property<string>("TrackingNumber")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("OrderId");

                    b.HasIndex("ApplicationUserId");

                    b.HasIndex("ProductId");

                    b.ToTable("Orders");
                });

            modelBuilder.Entity("ECommerceApp.Models.Product", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("CategoryId")
                        .HasColumnType("int");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Discount")
                        .HasColumnType("int");

                    b.Property<string>("ImageUrl")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<decimal>("Price")
                        .HasColumnType("decimal(18, 2)");

                    b.Property<int>("Quantity")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("CategoryId");

                    b.ToTable("Products");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            CategoryId = 1,
                            Description = "The Samsung Galaxy S21 features a stunning 6.2-inch Dynamic AMOLED display and is powered by the Exynos 2100 processor. With a triple camera setup and 5G connectivity, it provides high-quality photos and seamless streaming. The sleek design and long battery life make it perfect for daily use.",
                            Discount = 10,
                            ImageUrl = "https://rajshahitech.com/wp-content/uploads/2023/12/2101-60481.jpg",
                            Name = "Samsung Galaxy S21",
                            Price = 79999m,
                            Quantity = 5
                        },
                        new
                        {
                            Id = 2,
                            CategoryId = 2,
                            Description = "A novel that explores the serious issues of rape and racial inequality through the eyes of a child in the 1930s South. It emphasizes the importance of moral integrity and empathy.",
                            Discount = 5,
                            ImageUrl = "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcQnQ472uYK5UOemhgGD7wk3xpWJr4oDbsuY-A&s",
                            Name = "To Kill a Mockingbird",
                            Price = 599m,
                            Quantity = 2
                        },
                        new
                        {
                            Id = 3,
                            CategoryId = 3,
                            Description = "This classic Levi's denim jacket is a timeless piece that can be worn year-round. Made with durable denim, it features a button-up front and chest pockets for a stylish look. Perfect for layering, it adds an edge to any outfit.",
                            Discount = 20,
                            ImageUrl = "https://dimg.dillards.com/is/image/DillardsZoom/main/levis-type-iii-sherpa-trucker-jacket/05120005_zi_mustard_wash.jpg",
                            Name = "Levi's Denim Jacket",
                            Price = 4999m,
                            Quantity = 10
                        },
                        new
                        {
                            Id = 4,
                            CategoryId = 5,
                            Description = "The Nike Air Max 270 is designed for comfort and style. Featuring a large Air unit in the heel for responsive cushioning, these shoes provide all-day comfort. The sleek design is perfect for both workouts and casual wear.",
                            Discount = 15,
                            ImageUrl = "https://rukminim2.flixcart.com/image/850/1000/xif0q/shoe/b/f/a/-original-imagx9kwcjzzjkej.jpeg?q=90&crop=false",
                            Name = "Nike Air Max 270",
                            Price = 10999m,
                            Quantity = 14
                        },
                        new
                        {
                            Id = 5,
                            CategoryId = 1,
                            Description = "The MacBook Air with Apple M1 chip offers breakthrough performance with an 8-core CPU and up to 18 hours of battery life. Its lightweight design and Retina display make it a perfect choice for professionals on the go.",
                            Discount = 10,
                            ImageUrl = "https://cdn.mos.cms.futurecdn.net/gPvyaz76tASn87RCGuSdDc-1200-80.jpg",
                            Name = "Apple MacBook Air M1",
                            Price = 99999m,
                            Quantity = 6
                        },
                        new
                        {
                            Id = 6,
                            CategoryId = 4,
                            Description = "Experience thrilling off-road adventures with this durable, high-speed remote-control monster truck. Built to handle rough terrains, it's perfect for kids and adults who love outdoor fun.",
                            Discount = 15,
                            ImageUrl = "https://rukminim2.flixcart.com/image/850/1000/xif0q/remote-control-toy/v/2/g/rock-crawler-rc-monster-truck-4wd-off-road-vehicle-multicolor-3-original-imafvhr45wdh2pry.jpeg?q=90&crop=false",
                            Name = "Remote-Control Off-Road Monster Truck",
                            Price = 2999m,
                            Quantity = 10
                        },
                        new
                        {
                            Id = 7,
                            CategoryId = 8,
                            Description = "An iconic film about the powerful Italian-American crime family of Don Vito Corleone, exploring themes of power, loyalty, and family.",
                            Discount = 5,
                            ImageUrl = "https://m.media-amazon.com/images/M/MV5BYTJkNGQyZDgtZDQ0NC00MDM0LWEzZWQtYzUzZDEwMDljZWNjXkEyXkFqcGc@._V1_.jpg",
                            Name = "The Godfather (DVD)",
                            Price = 999m,
                            Quantity = 30
                        },
                        new
                        {
                            Id = 8,
                            CategoryId = 6,
                            Description = "Delicious and crispy French fries, perfect as a side or snack. Made with fresh potatoes and cooked to golden perfection, they pair well with any meal.",
                            Discount = 5,
                            ImageUrl = "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcTCtQU2Pf3U32qKKJLQB0tY83xthaYEYTatRw&s",
                            Name = "French Fries",
                            Price = 299m,
                            Quantity = 25
                        },
                        new
                        {
                            Id = 9,
                            CategoryId = 7,
                            Description = "Pepsi is a refreshing soft drink that provides a burst of flavor. Perfect for parties or as a quick refreshment on a hot day.",
                            Discount = 0,
                            ImageUrl = "https://www.jiomart.com/images/product/original/491208775/pepsi-750-ml-product-images-o491208775-p491208775-0-202203170727.jpg?im=Resize=(1000,1000)",
                            Name = "Pepsi",
                            Price = 30m,
                            Quantity = 40
                        },
                        new
                        {
                            Id = 10,
                            CategoryId = 1,
                            Description = "The Fitbit Charge 4 is a fitness tracker that helps you monitor your activity levels, heart rate, and sleep patterns. With built-in GPS and long battery life, it’s perfect for fitness enthusiasts.",
                            Discount = 10,
                            ImageUrl = "https://cdn.mos.cms.futurecdn.net/LaBALQRveyNDQjrac9HjC9.jpg",
                            Name = "Fitbit Charge 4",
                            Price = 14999m,
                            Quantity = 14
                        },
                        new
                        {
                            Id = 11,
                            CategoryId = 1,
                            Description = "The Acer Aspire 5 Slim Laptop is designed for students and professionals who need reliable performance in a lightweight build. It features a 15.6-inch Full HD IPS display for vibrant visuals and is powered by an Intel Core i5 processor, coupled with 8GB RAM and a 512GB SSD, delivering smooth multitasking and fast boot times. With up to 8 hours of battery life, it’s ideal for working on the go. It also includes a backlit keyboard, built-in webcam, and an advanced cooling system to keep it running efficiently even under heavy workloads.",
                            Discount = 5,
                            ImageUrl = "https://m.media-amazon.com/images/I/71howy7q+HL.jpg",
                            Name = "Acer Aspire 5 Slim Laptop",
                            Price = 49999m,
                            Quantity = 5
                        },
                        new
                        {
                            Id = 12,
                            CategoryId = 1,
                            Description = "The Samsung Galaxy Tab S7 is a powerful tablet featuring a stunning 11-inch display and the latest Snapdragon processor. Ideal for both entertainment and productivity.",
                            Discount = 15,
                            ImageUrl = "https://5.imimg.com/data5/SELLER/Default/2020/10/XD/SN/JS/51517768/samsung-tab-s7-27-81-cm-11-inch-6-gb-128-gb-wifi-mystic-black--500x500.jpg",
                            Name = "Samsung Galaxy Tab S7",
                            Price = 61999m,
                            Quantity = 8
                        },
                        new
                        {
                            Id = 13,
                            CategoryId = 5,
                            Description = "Puma Running Shoes are designed for comfort and performance. With a lightweight design and excellent grip, they are perfect for runners of all levels.",
                            Discount = 10,
                            ImageUrl = "https://za.puma.com/dw/image/v2/BDSF_PRD/on/demandware.static/-/Sites-ZA-Library/default/dwb6844bf2/Fast-R2.jpg?q=80&sw=720",
                            Name = "Puma Running Shoes",
                            Price = 5999m,
                            Quantity = 10
                        },
                        new
                        {
                            Id = 14,
                            CategoryId = 2,
                            Description = "The first book in the Harry Potter series introduces readers to a young boy who discovers he is a wizard and attends Hogwarts School of Witchcraft and Wizardry, embarking on magical adventures.",
                            Discount = 15,
                            ImageUrl = "https://images-na.ssl-images-amazon.com/images/I/81iqZ2HHD-L.jpg",
                            Name = "Harry Potter and the Sorcerer's Stone",
                            Price = 499m,
                            Quantity = 4
                        },
                        new
                        {
                            Id = 15,
                            CategoryId = 1,
                            Description = "The Asus ROG Zephyrus G14 is a gaming laptop that offers exceptional performance with its Ryzen 9 processor and high-refresh-rate display. Perfect for gamers who demand power and portability.",
                            Discount = 8,
                            ImageUrl = "https://m.media-amazon.com/images/I/71qbOU164lL._AC_UF1000,1000_QL80_.jpg",
                            Name = "Asus ROG Zephyrus G14",
                            Price = 149999m,
                            Quantity = 6
                        },
                        new
                        {
                            Id = 16,
                            CategoryId = 7,
                            Description = "Coca Cola is a classic soft drink that delivers a unique taste. Enjoy it chilled with friends or during family gatherings.",
                            Discount = 0,
                            ImageUrl = "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcT3M-xR4PTf-oldWP8s2UWBCDxtZatNqXs1NA&s",
                            Name = "Coca Cola",
                            Price = 15m,
                            Quantity = 20
                        },
                        new
                        {
                            Id = 17,
                            CategoryId = 3,
                            Description = "This Nike sportswear hoodie is perfect for workouts or casual wear. Made with soft fabric, it keeps you warm while providing style.",
                            Discount = 15,
                            ImageUrl = "https://images-cdn.ubuy.co.in/6699915c9d25aa62af63d3d6-nike-sportswear-boys-39-club-pullover.jpg",
                            Name = "Nike Sportswear Hoodie",
                            Price = 3999m,
                            Quantity = 0
                        },
                        new
                        {
                            Id = 18,
                            CategoryId = 4,
                            Description = "A Nerf blaster that fires darts up to 90 feet. It features a rotating drum that holds up to 6 darts, providing quick reloads and rapid firing.",
                            Discount = 10,
                            ImageUrl = "https://m.media-amazon.com/images/I/51LfZ+hdD5L.jpg",
                            Name = "Nerf N-Strike Elite Disruptor",
                            Price = 1499m,
                            Quantity = 0
                        },
                        new
                        {
                            Id = 19,
                            CategoryId = 6,
                            Description = "Hershey's chocolate is a beloved treat that is perfect for any occasion. Enjoy its rich and creamy taste in various flavors.",
                            Discount = 8,
                            ImageUrl = "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcQmatVcE5DIhAEjMm69GwVJu0Ge_UqwKVkvSQ&s",
                            Name = "Hershey's Chocolate",
                            Price = 150m,
                            Quantity = 0
                        },
                        new
                        {
                            Id = 20,
                            CategoryId = 8,
                            Description = "Inception is a sci-fi thriller directed by Christopher Nolan. The story follows a skilled thief who steals secrets from deep within the subconscious during the dream state. This mind-bending film combines stunning visuals with a complex narrative, exploring the themes of dreams and reality. With an all-star cast and a gripping score, Inception is a must-watch for any film enthusiast.",
                            Discount = 15,
                            ImageUrl = "https://m.media-amazon.com/images/I/912AErFSBHL._AC_UF894,1000_QL80_.jpg",
                            Name = "Inception",
                            Price = 999m,
                            Quantity = 6
                        });
                });

            modelBuilder.Entity("ECommerceApp.Models.Wishlist", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("ProductId")
                        .HasColumnType("int");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("ProductId");

                    b.HasIndex("UserId");

                    b.ToTable("Wishlists");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRole", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasDatabaseName("RoleNameIndex")
                        .HasFilter("[NormalizedName] IS NOT NULL");

                    b.ToTable("AspNetRoles", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("ClaimType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("RoleId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("ClaimType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.Property<string>("LoginProvider")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ProviderKey")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ProviderDisplayName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("RoleId")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("LoginProvider")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Value")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens", (string)null);
                });

            modelBuilder.Entity("ECommerceApp.Models.Cart", b =>
                {
                    b.HasOne("ECommerceApp.Models.Product", "product")
                        .WithMany()
                        .HasForeignKey("ProductId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ECommerceApp.Models.ApplicationUser", "applicationUser")
                        .WithMany()
                        .HasForeignKey("applicationUserId");

                    b.Navigation("applicationUser");

                    b.Navigation("product");
                });

            modelBuilder.Entity("ECommerceApp.Models.Order", b =>
                {
                    b.HasOne("ECommerceApp.Models.ApplicationUser", "ApplicationUser")
                        .WithMany()
                        .HasForeignKey("ApplicationUserId");

                    b.HasOne("ECommerceApp.Models.Product", "Product")
                        .WithMany()
                        .HasForeignKey("ProductId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("ApplicationUser");

                    b.Navigation("Product");
                });

            modelBuilder.Entity("ECommerceApp.Models.Product", b =>
                {
                    b.HasOne("ECommerceApp.Models.Category", "Category")
                        .WithMany("Products")
                        .HasForeignKey("CategoryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Category");
                });

            modelBuilder.Entity("ECommerceApp.Models.Wishlist", b =>
                {
                    b.HasOne("ECommerceApp.Models.Product", "Product")
                        .WithMany()
                        .HasForeignKey("ProductId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ECommerceApp.Models.ApplicationUser", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Product");

                    b.Navigation("User");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.HasOne("ECommerceApp.Models.ApplicationUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("ECommerceApp.Models.ApplicationUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ECommerceApp.Models.ApplicationUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.HasOne("ECommerceApp.Models.ApplicationUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("ECommerceApp.Models.Category", b =>
                {
                    b.Navigation("Products");
                });
#pragma warning restore 612, 618
        }
    }
}
