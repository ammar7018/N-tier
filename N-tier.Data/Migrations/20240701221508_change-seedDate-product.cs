using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace N_tier.Data.Migrations
{
    /// <inheritdoc />
    public partial class changeseedDateproduct : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "product",
                keyColumn: "Id",
                keyValue: 4,
                column: "CategoryId",
                value: 3);

            migrationBuilder.UpdateData(
                table: "product",
                keyColumn: "Id",
                keyValue: 6,
                column: "CategoryId",
                value: 3);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "product",
                keyColumn: "Id",
                keyValue: 4,
                column: "CategoryId",
                value: 5);

            migrationBuilder.UpdateData(
                table: "product",
                keyColumn: "Id",
                keyValue: 6,
                column: "CategoryId",
                value: 11);
        }
    }
}
