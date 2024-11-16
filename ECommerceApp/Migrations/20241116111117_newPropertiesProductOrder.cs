using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ECommerceApp.Migrations
{
    /// <inheritdoc />
    public partial class newPropertiesProductOrder : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Brand",
                table: "Products",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "ReturnExchangePolicyDays",
                table: "Products",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "DeliveryCharge",
                table: "Orders",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeliveryDate",
                table: "Orders",
                type: "datetime2",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "Brand", "ReturnExchangePolicyDays" },
                values: new object[] { "", 0 });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "Brand", "ReturnExchangePolicyDays" },
                values: new object[] { "", 0 });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "Brand", "ReturnExchangePolicyDays" },
                values: new object[] { "", 0 });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "Brand", "ReturnExchangePolicyDays" },
                values: new object[] { "", 0 });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "Brand", "ReturnExchangePolicyDays" },
                values: new object[] { "", 0 });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "Brand", "ReturnExchangePolicyDays" },
                values: new object[] { "", 0 });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "Brand", "ReturnExchangePolicyDays" },
                values: new object[] { "", 0 });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 8,
                columns: new[] { "Brand", "ReturnExchangePolicyDays" },
                values: new object[] { "", 0 });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 9,
                columns: new[] { "Brand", "ReturnExchangePolicyDays" },
                values: new object[] { "", 0 });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 10,
                columns: new[] { "Brand", "ReturnExchangePolicyDays" },
                values: new object[] { "", 0 });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 11,
                columns: new[] { "Brand", "ReturnExchangePolicyDays" },
                values: new object[] { "", 0 });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 12,
                columns: new[] { "Brand", "ReturnExchangePolicyDays" },
                values: new object[] { "", 0 });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 13,
                columns: new[] { "Brand", "ReturnExchangePolicyDays" },
                values: new object[] { "", 0 });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 14,
                columns: new[] { "Brand", "ReturnExchangePolicyDays" },
                values: new object[] { "", 0 });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 15,
                columns: new[] { "Brand", "ReturnExchangePolicyDays" },
                values: new object[] { "", 0 });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 16,
                columns: new[] { "Brand", "ReturnExchangePolicyDays" },
                values: new object[] { "", 0 });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 17,
                columns: new[] { "Brand", "ReturnExchangePolicyDays" },
                values: new object[] { "", 0 });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 18,
                columns: new[] { "Brand", "ReturnExchangePolicyDays" },
                values: new object[] { "", 0 });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 19,
                columns: new[] { "Brand", "ReturnExchangePolicyDays" },
                values: new object[] { "", 0 });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 20,
                columns: new[] { "Brand", "ReturnExchangePolicyDays" },
                values: new object[] { "", 0 });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Brand",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "ReturnExchangePolicyDays",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "DeliveryCharge",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "DeliveryDate",
                table: "Orders");
        }
    }
}
