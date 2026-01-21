using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Webshop.Migrations
{
    /// <inheritdoc />
    public partial class AddedSupplier : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Supplier",
                schema: "webshop",
                table: "Products");

            migrationBuilder.AddColumn<int>(
                name: "SupplierId",
                schema: "webshop",
                table: "Products",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Supplier",
                schema: "webshop",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Supplier", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Products_SupplierId",
                schema: "webshop",
                table: "Products",
                column: "SupplierId");

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Products_Supplier_SupplierId",
                schema: "webshop",
                table: "Products");

            migrationBuilder.DropTable(
                name: "Supplier",
                schema: "webshop");

            migrationBuilder.DropIndex(
                name: "IX_Products_SupplierId",
                schema: "webshop",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "SupplierId",
                schema: "webshop",
                table: "Products");

            migrationBuilder.AddColumn<string>(
                name: "Supplier",
                schema: "webshop",
                table: "Products",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
