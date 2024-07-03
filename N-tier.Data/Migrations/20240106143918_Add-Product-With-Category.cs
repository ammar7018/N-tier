using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace N_tier.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddProductWithCategory : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CategoryId",
                table: "product",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.UpdateData(
                table: "product",
                keyColumn: "Id",
                keyValue: 1,
                column: "CategoryId",
                value: 2);

            migrationBuilder.UpdateData(
                table: "product",
                keyColumn: "Id",
                keyValue: 2,
                column: "CategoryId",
                value: 1);

            migrationBuilder.UpdateData(
                table: "product",
                keyColumn: "Id",
                keyValue: 3,
                column: "CategoryId",
                value: 1);

            migrationBuilder.UpdateData(
                table: "product",
                keyColumn: "Id",
                keyValue: 4,
                column: "CategoryId",
                value: 5);

            migrationBuilder.UpdateData(
                table: "product",
                keyColumn: "Id",
                keyValue: 5,
                column: "CategoryId",
                value: 3);

            migrationBuilder.UpdateData(
                table: "product",
                keyColumn: "Id",
                keyValue: 6,
                column: "CategoryId",
                value: 11);

            migrationBuilder.CreateIndex(
                name: "IX_product_CategoryId",
                table: "product",
                column: "CategoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_product_Category_CategoryId",
                table: "product",
                column: "CategoryId",
                principalTable: "Category",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_product_Category_CategoryId",
                table: "product");

            migrationBuilder.DropIndex(
                name: "IX_product_CategoryId",
                table: "product");

            migrationBuilder.DropColumn(
                name: "CategoryId",
                table: "product");
        }
    }
}
