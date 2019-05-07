using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SFood.DataAccess.Migrator.Migrations
{
    public partial class CustomizationModuleRefactor : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "DishId",
                schema: "Dish",
                table: "CustomizationCategories",
                newName: "FromId");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                schema: "Restaurant",
                table: "Menus",
                maxLength: 100,
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 20,
                oldNullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDefault",
                schema: "Restaurant",
                table: "Menus",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedTime",
                schema: "HawkerCenter",
                table: "Seats",
                nullable: false,
                defaultValueSql: "getutcdate()");

            migrationBuilder.AddColumn<string>(
                name: "CreatedUserName",
                schema: "HawkerCenter",
                table: "Seats",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                schema: "HawkerCenter",
                table: "SeatAreas",
                maxLength: 20,
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 32,
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CreatedUserName",
                schema: "HawkerCenter",
                table: "SeatAreas",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "TargetUrl",
                schema: "HawkerCenter",
                table: "Banners",
                maxLength: 1000,
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "CreatedUserName",
                schema: "HawkerCenter",
                table: "Banners",
                maxLength: 100,
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "CenterId",
                schema: "HawkerCenter",
                table: "Banners",
                maxLength: 32,
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                schema: "Dish",
                table: "Customizations",
                maxLength: 100,
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 20,
                oldNullable: true);

            migrationBuilder.AddColumn<byte>(
                name: "Index",
                schema: "Dish",
                table: "Customizations",
                nullable: false,
                defaultValue: (byte)0);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                schema: "Dish",
                table: "CustomizationCategories",
                maxLength: 100,
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 20,
                oldNullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsMultiple",
                schema: "Dish",
                table: "CustomizationCategories",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsSelected",
                schema: "Dish",
                table: "CustomizationCategories",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsSystem",
                schema: "Dish",
                table: "CustomizationCategories",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateTable(
                name: "Dishes&CustomizationCategories",
                schema: "RelationShip",
                columns: table => new
                {
                    Id = table.Column<string>(maxLength: 32, nullable: false),
                    DishId = table.Column<string>(nullable: true),
                    CustomizationCategoryId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Dishes&CustomizationCategories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Dishes&CustomizationCategories_Dishes_DishId",
                        column: x => x.DishId,
                        principalSchema: "Restaurant",
                        principalTable: "Dishes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Seats_SeatAreaId",
                schema: "HawkerCenter",
                table: "Seats",
                column: "SeatAreaId");

            migrationBuilder.CreateIndex(
                name: "IX_Dishes&CustomizationCategories_DishId_CustomizationCategoryId",
                schema: "RelationShip",
                table: "Dishes&CustomizationCategories",
                columns: new[] { "DishId", "CustomizationCategoryId" });

            migrationBuilder.AddForeignKey(
                name: "FK_Banners_BasicInfos_CenterId",
                schema: "HawkerCenter",
                table: "Banners",
                column: "CenterId",
                principalSchema: "HawkerCenter",
                principalTable: "BasicInfos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_SeatAreas_BasicInfos_CenterId",
                schema: "HawkerCenter",
                table: "SeatAreas",
                column: "CenterId",
                principalSchema: "HawkerCenter",
                principalTable: "BasicInfos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Seats_SeatAreas_SeatAreaId",
                schema: "HawkerCenter",
                table: "Seats",
                column: "SeatAreaId",
                principalSchema: "HawkerCenter",
                principalTable: "SeatAreas",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Banners_BasicInfos_CenterId",
                schema: "HawkerCenter",
                table: "Banners");

            migrationBuilder.DropForeignKey(
                name: "FK_SeatAreas_BasicInfos_CenterId",
                schema: "HawkerCenter",
                table: "SeatAreas");

            migrationBuilder.DropForeignKey(
                name: "FK_Seats_SeatAreas_SeatAreaId",
                schema: "HawkerCenter",
                table: "Seats");

            migrationBuilder.DropTable(
                name: "Dishes&CustomizationCategories",
                schema: "RelationShip");

            migrationBuilder.DropIndex(
                name: "IX_Seats_SeatAreaId",
                schema: "HawkerCenter",
                table: "Seats");

            migrationBuilder.DropColumn(
                name: "IsDefault",
                schema: "Restaurant",
                table: "Menus");

            migrationBuilder.DropColumn(
                name: "CreatedTime",
                schema: "HawkerCenter",
                table: "Seats");

            migrationBuilder.DropColumn(
                name: "CreatedUserName",
                schema: "HawkerCenter",
                table: "Seats");

            migrationBuilder.DropColumn(
                name: "CreatedUserName",
                schema: "HawkerCenter",
                table: "SeatAreas");

            migrationBuilder.DropColumn(
                name: "Index",
                schema: "Dish",
                table: "Customizations");

            migrationBuilder.DropColumn(
                name: "IsMultiple",
                schema: "Dish",
                table: "CustomizationCategories");

            migrationBuilder.DropColumn(
                name: "IsSelected",
                schema: "Dish",
                table: "CustomizationCategories");

            migrationBuilder.DropColumn(
                name: "IsSystem",
                schema: "Dish",
                table: "CustomizationCategories");

            migrationBuilder.RenameColumn(
                name: "FromId",
                schema: "Dish",
                table: "CustomizationCategories",
                newName: "DishId");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                schema: "Restaurant",
                table: "Menus",
                maxLength: 20,
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 100,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                schema: "HawkerCenter",
                table: "SeatAreas",
                maxLength: 32,
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 20,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "TargetUrl",
                schema: "HawkerCenter",
                table: "Banners",
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 1000,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "CreatedUserName",
                schema: "HawkerCenter",
                table: "Banners",
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 100,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "CenterId",
                schema: "HawkerCenter",
                table: "Banners",
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 32,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                schema: "Dish",
                table: "Customizations",
                maxLength: 20,
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 100,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                schema: "Dish",
                table: "CustomizationCategories",
                maxLength: 20,
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 100,
                oldNullable: true);
        }
    }
}
