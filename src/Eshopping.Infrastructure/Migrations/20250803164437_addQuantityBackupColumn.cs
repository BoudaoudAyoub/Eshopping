using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Eshopping.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class addQuantityBackupColumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "QuantityBackup",
                table: "Products",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "QuantityBackup",
                table: "Products");
        }
    }
}
