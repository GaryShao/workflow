using Microsoft.EntityFrameworkCore.Migrations;

namespace SFood.DataAccess.Migrator.Migrations
{
    public partial class AddCountryCodeId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CountryCodeId",
                schema: "Restaurant",
                table: "Details",
                maxLength: 32,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CountryCodeId",
                schema: "IdentitySchema",
                table: "UserExtensions",
                maxLength: 32,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CountryCodeId",
                schema: "Restaurant",
                table: "Details");

            migrationBuilder.DropColumn(
                name: "CountryCodeId",
                schema: "IdentitySchema",
                table: "UserExtensions");
        }
    }
}
