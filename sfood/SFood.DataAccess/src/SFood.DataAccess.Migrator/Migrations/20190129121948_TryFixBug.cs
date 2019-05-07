using Microsoft.EntityFrameworkCore.Migrations;

namespace SFood.DataAccess.Migrator.Migrations
{
    public partial class TryFixBug : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Status",
                schema: "Restaurant",
                table: "BasicInfos",
                nullable: false,
                oldClrType: typeof(string),
                oldDefaultValue: "Running");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Status",
                schema: "Restaurant",
                table: "BasicInfos",
                nullable: false,
                defaultValue: "Running",
                oldClrType: typeof(string));
        }
    }
}
