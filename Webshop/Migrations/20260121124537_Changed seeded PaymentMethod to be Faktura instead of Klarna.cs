using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Webshop.Migrations
{
    /// <inheritdoc />
    public partial class ChangedseededPaymentMethodtobeFakturainsteadofKlarna : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                schema: "webshop",
                table: "PaymentMethods",
                keyColumn: "Id",
                keyValue: 3,
                column: "Name",
                value: "Faktura");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                schema: "webshop",
                table: "PaymentMethods",
                keyColumn: "Id",
                keyValue: 3,
                column: "Name",
                value: "Klarna");
        }
    }
}
