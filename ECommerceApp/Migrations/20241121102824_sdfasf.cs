using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ECommerceApp.Migrations
{
    /// <inheritdoc />
    public partial class sdfasf : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
        IF EXISTS (
            SELECT 1
            FROM INFORMATION_SCHEMA.COLUMNS
            WHERE TABLE_NAME = 'Orders' AND COLUMN_NAME = 'DeliveryCharge'
        )
        BEGIN
            ALTER TABLE [Orders] DROP COLUMN DeliveryCharge;
        END;
    ");

            // Drop DeliveryDate if it exists
            migrationBuilder.Sql(@"
        IF EXISTS (
            SELECT 1
            FROM INFORMATION_SCHEMA.COLUMNS
            WHERE TABLE_NAME = 'Orders' AND COLUMN_NAME = 'DeliveryDate'
        )
        BEGIN
            ALTER TABLE [Orders] DROP COLUMN DeliveryDate;
        END;
    ");
        }
    }
}
