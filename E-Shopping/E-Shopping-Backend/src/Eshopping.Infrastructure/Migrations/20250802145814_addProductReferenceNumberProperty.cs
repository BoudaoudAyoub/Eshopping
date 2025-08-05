using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Eshopping.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class addProductReferenceNumberProperty : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ReferenceNumber",
                table: "Products",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Products_ReferenceNumber",
                table: "Products",
                column: "ReferenceNumber",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Products_ReferenceNumber",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "ReferenceNumber",
                table: "Products");
        }
    }
}
