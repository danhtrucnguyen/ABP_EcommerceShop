using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ecommerce_Shop.Migrations
{
    /// <inheritdoc />
    public partial class Update_Category_Schema : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "AppCategories",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Description",
                table: "AppCategories");
        }
    }
}
