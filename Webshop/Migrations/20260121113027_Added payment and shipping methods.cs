using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Webshop.Migrations
{
    /// <inheritdoc />
    public partial class Addedpaymentandshippingmethods : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "PaymentMethodId",
                schema: "webshop",
                table: "Orders",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "PaymentStatus",
                schema: "webshop",
                table: "Orders",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "PaymentTransactionId",
                schema: "webshop",
                table: "Orders",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "ShippingCost",
                schema: "webshop",
                table: "Orders",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<int>(
                name: "ShippingMethodId",
                schema: "webshop",
                table: "Orders",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "PaymentMethods",
                schema: "webshop",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Provider = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    TransactionFee = table.Column<decimal>(type: "decimal(18,2)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PaymentMethods", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ShippingMethods",
                schema: "webshop",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    EstimatedDaysMin = table.Column<int>(type: "int", nullable: false),
                    EstimatedDaysMax = table.Column<int>(type: "int", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ShippingMethods", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Orders_PaymentMethodId",
                schema: "webshop",
                table: "Orders",
                column: "PaymentMethodId");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_ShippingMethodId",
                schema: "webshop",
                table: "Orders",
                column: "ShippingMethodId");

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

            migrationBuilder.DropTable(
                name: "PaymentMethods",
                schema: "webshop");

            migrationBuilder.DropTable(
                name: "ShippingMethods",
                schema: "webshop");

            migrationBuilder.DropIndex(
                name: "IX_Orders_PaymentMethodId",
                schema: "webshop",
                table: "Orders");

            migrationBuilder.DropIndex(
                name: "IX_Orders_ShippingMethodId",
                schema: "webshop",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "PaymentMethodId",
                schema: "webshop",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "PaymentStatus",
                schema: "webshop",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "PaymentTransactionId",
                schema: "webshop",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "ShippingCost",
                schema: "webshop",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "ShippingMethodId",
                schema: "webshop",
                table: "Orders");
        }
    }
}
