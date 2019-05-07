using Microsoft.EntityFrameworkCore.Migrations;

namespace SFood.DataAccess.Migrator.Migrations
{
    public partial class CustomizationModification : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Customizations_Dishes_DishId",
                schema: "Dish",
                table: "Customizations");

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                schema: "Dish",
                table: "Customizations",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AlterColumn<string>(
                name: "RestaurantId",
                schema: "Dish",
                table: "CustomizationCategories",
                maxLength: 32,
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "RestaurantCategoryId",
                schema: "Dish",
                table: "CustomizationCategories",
                maxLength: 32,
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AddColumn<byte>(
                name: "Index",
                schema: "Dish",
                table: "CustomizationCategories",
                nullable: false,
                defaultValue: (byte)0);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                schema: "Dish",
                table: "CustomizationCategories",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateIndex(
                name: "IX_CustomizationCategories_RestaurantId",
                schema: "Dish",
                table: "CustomizationCategories",
                column: "RestaurantId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_CustomizationCategories_RestaurantId",
                schema: "Dish",
                table: "CustomizationCategories");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                schema: "Dish",
                table: "Customizations");

            migrationBuilder.DropColumn(
                name: "Index",
                schema: "Dish",
                table: "CustomizationCategories");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                schema: "Dish",
                table: "CustomizationCategories");

            migrationBuilder.AlterColumn<string>(
                name: "RestaurantId",
                schema: "Dish",
                table: "CustomizationCategories",
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 32,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "RestaurantCategoryId",
                schema: "Dish",
                table: "CustomizationCategories",
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 32,
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Customizations_Dishes_DishId",
                schema: "Dish",
                table: "Customizations",
                column: "DishId",
                principalSchema: "Restaurant",
                principalTable: "Dishes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
