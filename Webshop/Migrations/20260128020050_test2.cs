using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Webshop.Migrations
{
    /// <inheritdoc />
    public partial class test2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                schema: "webshop",
                table: "PaymentMethods",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                schema: "webshop",
                table: "PaymentMethods",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                schema: "webshop",
                table: "PaymentMethods",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                schema: "webshop",
                table: "ShippingMethods",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                schema: "webshop",
                table: "ShippingMethods",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                schema: "webshop",
                table: "ShippingMethods",
                keyColumn: "Id",
                keyValue: 3);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                schema: "webshop",
                table: "PaymentMethods",
                columns: new[] { "Id", "IsActive", "Name", "Provider", "TransactionFee" },
                values: new object[,]
                {
                    { 1, true, "Kort", "Klarna", 0m },
                    { 2, true, "Swish", "Swish", 0m },
                    { 3, true, "Faktura", "Klarna", 29m }
                });

            migrationBuilder.InsertData(
                schema: "webshop",
                table: "ShippingMethods",
                columns: new[] { "Id", "Description", "EstimatedDaysMax", "EstimatedDaysMin", "IsActive", "Name", "Price" },
                values: new object[,]
                {
                    { 1, "3-5 arbetsdagar", 5, 3, true, "Standard", 49m },
                    { 2, "1-2 arbetsdagar", 2, 1, true, "Express", 99m },
                    { 3, "Hämta direkt", 0, 0, true, "Hämta i butik", 0m }
                });
        }
    }
}
