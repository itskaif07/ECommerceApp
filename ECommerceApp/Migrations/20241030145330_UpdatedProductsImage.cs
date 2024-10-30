using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ECommerceApp.Migrations
{
    /// <inheritdoc />
    public partial class UpdatedProductsImage : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Profile",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UserName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PhoneNumber = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    City = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    State = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Pincode = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Profile", x => x.Id);
                });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 1,
                column: "ImageUrl",
                value: "https://rajshahitech.com/wp-content/uploads/2023/12/2101-60481.jpg");

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 2,
                column: "ImageUrl",
                value: "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcQnQ472uYK5UOemhgGD7wk3xpWJr4oDbsuY-A&s");

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 3,
                column: "ImageUrl",
                value: "https://dimg.dillards.com/is/image/DillardsZoom/main/levis-type-iii-sherpa-trucker-jacket/05120005_zi_mustard_wash.jpg");

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 4,
                column: "ImageUrl",
                value: "https://rukminim2.flixcart.com/image/850/1000/xif0q/shoe/b/f/a/-original-imagx9kwcjzzjkej.jpeg?q=90&crop=false");

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 5,
                column: "ImageUrl",
                value: "https://cdn.mos.cms.futurecdn.net/gPvyaz76tASn87RCGuSdDc-1200-80.jpg");

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "CategoryId", "Description", "Discount", "ImageUrl", "Name", "Price" },
                values: new object[] { 4, "Experience thrilling off-road adventures with this durable, high-speed remote-control monster truck. Built to handle rough terrains, it's perfect for kids and adults who love outdoor fun.", 15, "https://rukminim2.flixcart.com/image/850/1000/xif0q/remote-control-toy/v/2/g/rock-crawler-rc-monster-truck-4wd-off-road-vehicle-multicolor-3-original-imafvhr45wdh2pry.jpeg?q=90&crop=false", "Remote-Control Off-Road Monster Truck", 2999m });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 7,
                column: "ImageUrl",
                value: "https://m.media-amazon.com/images/M/MV5BYTJkNGQyZDgtZDQ0NC00MDM0LWEzZWQtYzUzZDEwMDljZWNjXkEyXkFqcGc@._V1_.jpg");

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 8,
                column: "ImageUrl",
                value: "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcTCtQU2Pf3U32qKKJLQB0tY83xthaYEYTatRw&s");

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 9,
                column: "ImageUrl",
                value: "https://www.jiomart.com/images/product/original/491208775/pepsi-750-ml-product-images-o491208775-p491208775-0-202203170727.jpg?im=Resize=(1000,1000)");

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 10,
                column: "ImageUrl",
                value: "https://cdn.mos.cms.futurecdn.net/LaBALQRveyNDQjrac9HjC9.jpg");

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 12,
                column: "ImageUrl",
                value: "https://5.imimg.com/data5/SELLER/Default/2020/10/XD/SN/JS/51517768/samsung-tab-s7-27-81-cm-11-inch-6-gb-128-gb-wifi-mystic-black--500x500.jpg");

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 13,
                column: "ImageUrl",
                value: "https://za.puma.com/dw/image/v2/BDSF_PRD/on/demandware.static/-/Sites-ZA-Library/default/dwb6844bf2/Fast-R2.jpg?q=80&sw=720");

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 15,
                column: "ImageUrl",
                value: "https://m.media-amazon.com/images/I/71qbOU164lL._AC_UF1000,1000_QL80_.jpg");

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 16,
                columns: new[] { "ImageUrl", "Price" },
                values: new object[] { "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcT3M-xR4PTf-oldWP8s2UWBCDxtZatNqXs1NA&s", 15m });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 17,
                column: "ImageUrl",
                value: "https://images-cdn.ubuy.co.in/6699915c9d25aa62af63d3d6-nike-sportswear-boys-39-club-pullover.jpg");

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 18,
                column: "ImageUrl",
                value: "https://m.media-amazon.com/images/I/51LfZ+hdD5L.jpg");

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 19,
                column: "ImageUrl",
                value: "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcQmatVcE5DIhAEjMm69GwVJu0Ge_UqwKVkvSQ&s");

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 20,
                column: "ImageUrl",
                value: "https://m.media-amazon.com/images/I/912AErFSBHL._AC_UF894,1000_QL80_.jpg");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Profile");

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 1,
                column: "ImageUrl",
                value: "https://m.media-amazon.com/images/I/81G8g8Oc+nL.jpg");

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 2,
                column: "ImageUrl",
                value: "https://images-na.ssl-images-amazon.com/images/I/81Otw%2BqJgpL.jpg");

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 3,
                column: "ImageUrl",
                value: "https://m.media-amazon.com/images/I/71i7ZzHcE7L.jpg");

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 4,
                column: "ImageUrl",
                value: "https://m.media-amazon.com/images/I/61uR+Zf+UFL.jpg");

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 5,
                column: "ImageUrl",
                value: "https://m.media-amazon.com/images/I/71WsZPOoq7L.jpg");

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "CategoryId", "Description", "Discount", "ImageUrl", "Name", "Price" },
                values: new object[] { 1, "The LG 43-inch 4K UHD Smart TV offers stunning picture quality and vibrant colors. With built-in Wi-Fi and streaming capabilities, you can enjoy your favorite shows and movies in high definition.", 12, "https://m.media-amazon.com/images/I/61r8h1Hh0AL._SL1500_.jpg", "LG 43-inch 4K UHD Smart TV", 44999m });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 7,
                column: "ImageUrl",
                value: "https://images-na.ssl-images-amazon.com/images/I/81xJgUt5ToL.jpg");

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 8,
                column: "ImageUrl",
                value: "https://m.media-amazon.com/images/I/71Uu9FG-9xL.jpg");

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 9,
                column: "ImageUrl",
                value: "https://m.media-amazon.com/images/I/71bXqKOM5eL.jpg");

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 10,
                column: "ImageUrl",
                value: "https://m.media-amazon.com/images/I/71e+JH03ZTL.jpg");

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 12,
                column: "ImageUrl",
                value: "https://m.media-amazon.com/images/I/71H62C4G7wL.jpg");

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 13,
                column: "ImageUrl",
                value: "https://m.media-amazon.com/images/I/61JeD2t8P0L.jpg");

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 15,
                column: "ImageUrl",
                value: "https://m.media-amazon.com/images/I/71dHnMlD0ML.jpg");

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 16,
                columns: new[] { "ImageUrl", "Price" },
                values: new object[] { "https://m.media-amazon.com/images/I/71Z8VVCh6BL.jpg", 30m });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 17,
                column: "ImageUrl",
                value: "https://m.media-amazon.com/images/I/71DGFUeCrhL.jpg");

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 18,
                column: "ImageUrl",
                value: "https://images-na.ssl-images-amazon.com/images/I/81mKT3vE1jL.jpg");

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 19,
                column: "ImageUrl",
                value: "https://m.media-amazon.com/images/I/61FlCnbnKeL.jpg");

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 20,
                column: "ImageUrl",
                value: "https://m.media-amazon.com/images/I/81p+GqH1c4L.jpg");
        }
    }
}
