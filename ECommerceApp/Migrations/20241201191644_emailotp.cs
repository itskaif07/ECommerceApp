using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ECommerceApp.Migrations
{
    /// <inheritdoc />
    public partial class emailotp : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_UserOtps",
                table: "UserOtps");

            migrationBuilder.RenameTable(
                name: "UserOtps",
                newName: "userOtps");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "userOtps",
                newName: "userId");

            migrationBuilder.RenameColumn(
                name: "Otp",
                table: "userOtps",
                newName: "otp");

            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                table: "userOtps",
                newName: "createdAt");

            migrationBuilder.AddPrimaryKey(
                name: "PK_userOtps",
                table: "userOtps",
                column: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_userOtps",
                table: "userOtps");

            migrationBuilder.RenameTable(
                name: "userOtps",
                newName: "UserOtps");

            migrationBuilder.RenameColumn(
                name: "userId",
                table: "UserOtps",
                newName: "UserId");

            migrationBuilder.RenameColumn(
                name: "otp",
                table: "UserOtps",
                newName: "Otp");

            migrationBuilder.RenameColumn(
                name: "createdAt",
                table: "UserOtps",
                newName: "CreatedAt");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserOtps",
                table: "UserOtps",
                column: "Id");
        }
    }
}
