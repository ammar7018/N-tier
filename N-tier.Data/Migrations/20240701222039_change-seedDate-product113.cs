using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace N_tier.Data.Migrations
{
    /// <inheritdoc />
    public partial class changeseedDateproduct113 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "product",
                keyColumn: "Id",
                keyValue: 1,
                column: "CategoryId",
                value: 3);

            migrationBuilder.UpdateData(
                table: "product",
                keyColumn: "Id",
                keyValue: 2,
                column: "CategoryId",
                value: 3);

            migrationBuilder.UpdateData(
                table: "product",
                keyColumn: "Id",
                keyValue: 3,
                column: "CategoryId",
                value: 2);

            migrationBuilder.UpdateData(
                table: "product",
                keyColumn: "Id",
                keyValue: 4,
                column: "CategoryId",
                value: 1);

            migrationBuilder.UpdateData(
                table: "product",
                keyColumn: "Id",
                keyValue: 5,
                column: "CategoryId",
                value: 2);

            migrationBuilder.UpdateData(
                table: "product",
                keyColumn: "Id",
                keyValue: 6,
                column: "CategoryId",
                value: 1);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
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
                value: 3);

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
                value: 3);
        }
    }
}
