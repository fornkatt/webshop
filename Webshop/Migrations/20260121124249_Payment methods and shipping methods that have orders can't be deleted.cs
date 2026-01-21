using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Webshop.Migrations
{
    /// <inheritdoc />
    public partial class Paymentmethodsandshippingmethodsthathaveorderscantbedeleted : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Orders_PaymentMethods_PaymentMethodId",
                schema: "webshop",
                table: "Orders");

            migrationBuilder.DropForeignKey(
                name: "FK_Orders_ShippingMethods_ShippingMethodId",
                schema: "webshop",
                table: "Orders");

            migrationBuilder.AlterColumn<int>(
                name: "ShippingMethodId",
                schema: "webshop",
                table: "Orders",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.InsertData(
                schema: "webshop",
                table: "PaymentMethods",
                columns: new[] { "Id", "IsActive", "Name", "Provider", "TransactionFee" },
                values: new object[,]
                {
                    { 1, true, "Kort", "Stripe", null },
                    { 2, true, "Swish", "Swish", null },
                    { 3, true, "Klarna", "Klarna", 29m }
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

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_PaymentMethods_PaymentMethodId",
                schema: "webshop",
                table: "Orders",
                column: "PaymentMethodId",
                principalSchema: "webshop",
                principalTable: "PaymentMethods",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_ShippingMethods_ShippingMethodId",
                schema: "webshop",
                table: "Orders",
                column: "ShippingMethodId",
                principalSchema: "webshop",
                principalTable: "ShippingMethods",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Orders_PaymentMethods_PaymentMethodId",
                schema: "webshop",
                table: "Orders");

            migrationBuilder.DropForeignKey(
                name: "FK_Orders_ShippingMethods_ShippingMethodId",
                schema: "webshop",
                table: "Orders");

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

            migrationBuilder.AlterColumn<int>(
                name: "ShippingMethodId",
                schema: "webshop",
                table: "Orders",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_PaymentMethods_PaymentMethodId",
                schema: "webshop",
                table: "Orders",
                column: "PaymentMethodId",
                principalSchema: "webshop",
                principalTable: "PaymentMethods",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_ShippingMethods_ShippingMethodId",
                schema: "webshop",
                table: "Orders",
                column: "ShippingMethodId",
                principalSchema: "webshop",
                principalTable: "ShippingMethods",
                principalColumn: "Id");
        }
    }
}
