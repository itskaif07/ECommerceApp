using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ECommerceApp.Migrations
{
    /// <inheritdoc />
    public partial class Categoryinwishlist : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CategoryId",
                table: "Wishlists",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Wishlists_CategoryId",
                table: "Wishlists",
                column: "CategoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_Wishlists_Categories_CategoryId",
                table: "Wishlists",
                column: "CategoryId",
                principalTable: "Categories",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Wishlists_Categories_CategoryId",
                table: "Wishlists");

            migrationBuilder.DropIndex(
                name: "IX_Wishlists_CategoryId",
                table: "Wishlists");

            migrationBuilder.DropColumn(
                name: "CategoryId",
                table: "Wishlists");
        }
    }
}
