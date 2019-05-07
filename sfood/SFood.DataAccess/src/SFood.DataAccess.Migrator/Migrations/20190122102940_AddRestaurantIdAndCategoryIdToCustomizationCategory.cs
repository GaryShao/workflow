using Microsoft.EntityFrameworkCore.Migrations;

namespace SFood.DataAccess.Migrator.Migrations
{
    public partial class AddRestaurantIdAndCategoryIdToCustomizationCategory : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "RestaurantCategoryId",
                schema: "Dish",
                table: "CustomizationCategories",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RestaurantId",
                schema: "Dish",
                table: "CustomizationCategories",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RestaurantCategoryId",
                schema: "Dish",
                table: "CustomizationCategories");

            migrationBuilder.DropColumn(
                name: "RestaurantId",
                schema: "Dish",
                table: "CustomizationCategories");
        }
    }
}
