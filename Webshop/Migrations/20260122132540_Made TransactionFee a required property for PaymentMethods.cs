using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Webshop.Migrations
{
    /// <inheritdoc />
    public partial class MadeTransactionFeearequiredpropertyforPaymentMethods : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "TransactionFee",
                schema: "webshop",
                table: "PaymentMethods",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)",
                oldNullable: true);

            migrationBuilder.UpdateData(
                schema: "webshop",
                table: "PaymentMethods",
                keyColumn: "Id",
                keyValue: 1,
                column: "TransactionFee",
                value: 0m);

            migrationBuilder.UpdateData(
                schema: "webshop",
                table: "PaymentMethods",
                keyColumn: "Id",
                keyValue: 2,
                column: "TransactionFee",
                value: 0m);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "TransactionFee",
                schema: "webshop",
                table: "PaymentMethods",
                type: "decimal(18,2)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");

            migrationBuilder.UpdateData(
                schema: "webshop",
                table: "PaymentMethods",
                keyColumn: "Id",
                keyValue: 1,
                column: "TransactionFee",
                value: null);

            migrationBuilder.UpdateData(
                schema: "webshop",
                table: "PaymentMethods",
                keyColumn: "Id",
                keyValue: 2,
                column: "TransactionFee",
                value: null);
        }
    }
}
