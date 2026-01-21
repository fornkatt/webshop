using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Webshop.Migrations
{
    /// <inheritdoc />
    public partial class AddedOrderOrderItemmodelsandaddedaSupplierDbSet : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Products_Categories_CategoryId",
                schema: "webshop",
                table: "Products");

            migrationBuilder.DropForeignKey(
                name: "FK_Products_Supplier_SupplierId",
                schema: "webshop",
                table: "Products");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Supplier",
                schema: "webshop",
                table: "Supplier");

            migrationBuilder.RenameTable(
                name: "Supplier",
                schema: "webshop",
                newName: "Suppliers",
                newSchema: "webshop");

            migrationBuilder.AddColumn<DateTime>(
                name: "DiscontinuedDate",
                schema: "webshop",
                table: "Products",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                schema: "webshop",
                table: "Products",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsDiscontinued",
                schema: "webshop",
                table: "Products",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Suppliers",
                schema: "webshop",
                table: "Suppliers",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "Orders",
                schema: "webshop",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CustomerId = table.Column<int>(type: "int", nullable: false),
                    OrderDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    TotalAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ShippingAddressId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Orders", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Orders_Addresses_ShippingAddressId",
                        column: x => x.ShippingAddressId,
                        principalSchema: "webshop",
                        principalTable: "Addresses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Orders_Customers_CustomerId",
                        column: x => x.CustomerId,
                        principalSchema: "webshop",
                        principalTable: "Customers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "OrderItems",
                schema: "webshop",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OrderId = table.Column<int>(type: "int", nullable: false),
                    ProductId = table.Column<int>(type: "int", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    UnitPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Subtotal = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ProductName = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OrderItems_Orders_OrderId",
                        column: x => x.OrderId,
                        principalSchema: "webshop",
                        principalTable: "Orders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OrderItems_Products_ProductId",
                        column: x => x.ProductId,
                        principalSchema: "webshop",
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_OrderItems_OrderId",
                schema: "webshop",
                table: "OrderItems",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderItems_ProductId",
                schema: "webshop",
                table: "OrderItems",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_CustomerId",
                schema: "webshop",
                table: "Orders",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_ShippingAddressId",
                schema: "webshop",
                table: "Orders",
                column: "ShippingAddressId");

            migrationBuilder.AddForeignKey(
                name: "FK_Products_Categories_CategoryId",
                schema: "webshop",
                table: "Products",
                column: "CategoryId",
                principalSchema: "webshop",
                principalTable: "Categories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Products_Suppliers_SupplierId",
                schema: "webshop",
                table: "Products",
                column: "SupplierId",
                principalSchema: "webshop",
                principalTable: "Suppliers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Products_Categories_CategoryId",
                schema: "webshop",
                table: "Products");

            migrationBuilder.DropForeignKey(
                name: "FK_Products_Suppliers_SupplierId",
                schema: "webshop",
                table: "Products");

            migrationBuilder.DropTable(
                name: "OrderItems",
                schema: "webshop");

            migrationBuilder.DropTable(
                name: "Orders",
                schema: "webshop");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Suppliers",
                schema: "webshop",
                table: "Suppliers");

            migrationBuilder.DropColumn(
                name: "DiscontinuedDate",
                schema: "webshop",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "IsActive",
                schema: "webshop",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "IsDiscontinued",
                schema: "webshop",
                table: "Products");

            migrationBuilder.RenameTable(
                name: "Suppliers",
                schema: "webshop",
                newName: "Supplier",
                newSchema: "webshop");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Supplier",
                schema: "webshop",
                table: "Supplier",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Products_Categories_CategoryId",
                schema: "webshop",
                table: "Products",
                column: "CategoryId",
                principalSchema: "webshop",
                principalTable: "Categories",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Products_Supplier_SupplierId",
                schema: "webshop",
                table: "Products",
                column: "SupplierId",
                principalSchema: "webshop",
                principalTable: "Supplier",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
