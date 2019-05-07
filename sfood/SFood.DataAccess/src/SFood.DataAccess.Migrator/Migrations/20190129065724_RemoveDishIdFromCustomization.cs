using Microsoft.EntityFrameworkCore.Migrations;

namespace SFood.DataAccess.Migrator.Migrations
{
    public partial class RemoveDishIdFromCustomization : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Customizations_DishId",
                schema: "Dish",
                table: "Customizations");

            migrationBuilder.DropColumn(
                name: "DishId",
                schema: "Dish",
                table: "Customizations");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "DishId",
                schema: "Dish",
                table: "Customizations",
                maxLength: 32,
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Customizations_DishId",
                schema: "Dish",
                table: "Customizations",
                column: "DishId");
        }
    }
}
