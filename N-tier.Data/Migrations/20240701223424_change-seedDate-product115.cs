using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace N_tier.Data.Migrations
{
    /// <inheritdoc />
    public partial class changeseedDateproduct115 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ID",
                table: "Category",
                newName: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Category",
                newName: "ID");
        }
    }
}
