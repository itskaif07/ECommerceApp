using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace ECommerceApp.Migrations
{
    /// <inheritdoc />
    public partial class InitialMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    City = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    State = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PinCode = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    UserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SecurityStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "bit", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "bit", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Categories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderKey = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Products",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Discount = table.Column<int>(type: "int", nullable: true),
                    CategoryId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Products_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Categories",
                        principalColumn: "Id");
                });

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Electronics" },
                    { 2, "Books" },
                    { 3, "Clothing" },
                    { 4, "Toys" },
                    { 5, "Shoes" },
                    { 6, "Foods" },
                    { 7, "Beverages" },
                    { 8, "Movies" }
                });

            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "Id", "CategoryId", "Description", "Discount", "ImageUrl", "Name", "Price" },
                values: new object[,]
                {
                    { 1, 1, "The Samsung Galaxy S21 features a stunning 6.2-inch Dynamic AMOLED display and is powered by the Exynos 2100 processor. With a triple camera setup and 5G connectivity, it provides high-quality photos and seamless streaming. The sleek design and long battery life make it perfect for daily use.", 10, "https://m.media-amazon.com/images/I/81G8g8Oc+nL.jpg", "Samsung Galaxy S21", 79999m },
                    { 2, 2, "A novel that explores the serious issues of rape and racial inequality through the eyes of a child in the 1930s South. It emphasizes the importance of moral integrity and empathy.", 5, "https://images-na.ssl-images-amazon.com/images/I/81Otw%2BqJgpL.jpg", "To Kill a Mockingbird", 599m },
                    { 3, 3, "This classic Levi's denim jacket is a timeless piece that can be worn year-round. Made with durable denim, it features a button-up front and chest pockets for a stylish look. Perfect for layering, it adds an edge to any outfit.", 20, "https://m.media-amazon.com/images/I/71i7ZzHcE7L.jpg", "Levi's Denim Jacket", 4999m },
                    { 4, 5, "The Nike Air Max 270 is designed for comfort and style. Featuring a large Air unit in the heel for responsive cushioning, these shoes provide all-day comfort. The sleek design is perfect for both workouts and casual wear.", 15, "https://m.media-amazon.com/images/I/61uR+Zf+UFL.jpg", "Nike Air Max 270", 10999m },
                    { 5, 1, "The MacBook Air with Apple M1 chip offers breakthrough performance with an 8-core CPU and up to 18 hours of battery life. Its lightweight design and Retina display make it a perfect choice for professionals on the go.", 10, "https://m.media-amazon.com/images/I/71WsZPOoq7L.jpg", "Apple MacBook Air M1", 99999m },
                    { 6, 1, "The LG 43-inch 4K UHD Smart TV offers stunning picture quality and vibrant colors. With built-in Wi-Fi and streaming capabilities, you can enjoy your favorite shows and movies in high definition.", 12, "https://m.media-amazon.com/images/I/61r8h1Hh0AL._SL1500_.jpg", "LG 43-inch 4K UHD Smart TV", 44999m },
                    { 7, 8, "An iconic film about the powerful Italian-American crime family of Don Vito Corleone, exploring themes of power, loyalty, and family.", 5, "https://images-na.ssl-images-amazon.com/images/I/81xJgUt5ToL.jpg", "The Godfather (DVD)", 999m },
                    { 8, 6, "Delicious and crispy French fries, perfect as a side or snack. Made with fresh potatoes and cooked to golden perfection, they pair well with any meal.", 5, "https://m.media-amazon.com/images/I/71Uu9FG-9xL.jpg", "French Fries", 299m },
                    { 9, 7, "Pepsi is a refreshing soft drink that provides a burst of flavor. Perfect for parties or as a quick refreshment on a hot day.", 0, "https://m.media-amazon.com/images/I/71bXqKOM5eL.jpg", "Pepsi", 30m },
                    { 10, 1, "The Fitbit Charge 4 is a fitness tracker that helps you monitor your activity levels, heart rate, and sleep patterns. With built-in GPS and long battery life, it’s perfect for fitness enthusiasts.", 10, "https://m.media-amazon.com/images/I/71e+JH03ZTL.jpg", "Fitbit Charge 4", 14999m },
                    { 11, 1, "The Acer Aspire 5 Slim Laptop is designed for students and professionals who need reliable performance in a lightweight build. It features a 15.6-inch Full HD IPS display for vibrant visuals and is powered by an Intel Core i5 processor, coupled with 8GB RAM and a 512GB SSD, delivering smooth multitasking and fast boot times. With up to 8 hours of battery life, it’s ideal for working on the go. It also includes a backlit keyboard, built-in webcam, and an advanced cooling system to keep it running efficiently even under heavy workloads.", 5, "https://m.media-amazon.com/images/I/71howy7q+HL.jpg", "Acer Aspire 5 Slim Laptop", 49999m },
                    { 12, 1, "The Samsung Galaxy Tab S7 is a powerful tablet featuring a stunning 11-inch display and the latest Snapdragon processor. Ideal for both entertainment and productivity.", 15, "https://m.media-amazon.com/images/I/71H62C4G7wL.jpg", "Samsung Galaxy Tab S7", 61999m },
                    { 13, 5, "Puma Running Shoes are designed for comfort and performance. With a lightweight design and excellent grip, they are perfect for runners of all levels.", 10, "https://m.media-amazon.com/images/I/61JeD2t8P0L.jpg", "Puma Running Shoes", 5999m },
                    { 14, 2, "The first book in the Harry Potter series introduces readers to a young boy who discovers he is a wizard and attends Hogwarts School of Witchcraft and Wizardry, embarking on magical adventures.", 15, "https://images-na.ssl-images-amazon.com/images/I/81iqZ2HHD-L.jpg", "Harry Potter and the Sorcerer's Stone", 499m },
                    { 15, 1, "The Asus ROG Zephyrus G14 is a gaming laptop that offers exceptional performance with its Ryzen 9 processor and high-refresh-rate display. Perfect for gamers who demand power and portability.", 8, "https://m.media-amazon.com/images/I/71dHnMlD0ML.jpg", "Asus ROG Zephyrus G14", 149999m },
                    { 16, 7, "Coca Cola is a classic soft drink that delivers a unique taste. Enjoy it chilled with friends or during family gatherings.", 0, "https://m.media-amazon.com/images/I/71Z8VVCh6BL.jpg", "Coca Cola", 30m },
                    { 17, 3, "This Nike sportswear hoodie is perfect for workouts or casual wear. Made with soft fabric, it keeps you warm while providing style.", 15, "https://m.media-amazon.com/images/I/71DGFUeCrhL.jpg", "Nike Sportswear Hoodie", 3999m },
                    { 18, 4, "A Nerf blaster that fires darts up to 90 feet. It features a rotating drum that holds up to 6 darts, providing quick reloads and rapid firing.", 10, "https://images-na.ssl-images-amazon.com/images/I/81mKT3vE1jL.jpg", "Nerf N-Strike Elite Disruptor", 1499m },
                    { 19, 6, "Hershey's chocolate is a beloved treat that is perfect for any occasion. Enjoy its rich and creamy taste in various flavors.", 5, "https://m.media-amazon.com/images/I/61FlCnbnKeL.jpg", "Hershey's Chocolate", 150m },
                    { 20, 8, "Inception is a sci-fi thriller directed by Christopher Nolan. The story follows a skilled thief who steals secrets from deep within the subconscious during the dream state. This mind-bending film combines stunning visuals with a complex narrative, exploring the themes of dreams and reality. With an all-star cast and a gripping score, Inception is a must-watch for any film enthusiast.", 15, "https://m.media-amazon.com/images/I/81p+GqH1c4L.jpg", "Inception", 999m }
                });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true,
                filter: "[NormalizedName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "AspNetUsers",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_Email",
                table: "AspNetUsers",
                column: "Email",
                unique: true,
                filter: "[Email] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_PhoneNumber",
                table: "AspNetUsers",
                column: "PhoneNumber",
                unique: true,
                filter: "[PhoneNumber] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true,
                filter: "[NormalizedUserName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Products_CategoryId",
                table: "Products",
                column: "CategoryId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "Products");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "Categories");
        }
    }
}
