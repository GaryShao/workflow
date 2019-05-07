using Microsoft.EntityFrameworkCore.Migrations;

namespace SFood.DataAccess.Migrator.Migrations
{
    public partial class AddLocalization : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "Localization");

            migrationBuilder.CreateTable(
                name: "Languages",
                schema: "Localization",
                columns: table => new
                {
                    Id = table.Column<string>(maxLength: 32, nullable: false),
                    Code = table.Column<string>(maxLength: 20, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Languages", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Resources",
                schema: "Localization",
                columns: table => new
                {
                    Id = table.Column<string>(maxLength: 32, nullable: false),
                    Key = table.Column<string>(maxLength: 100, nullable: true),
                    Value = table.Column<string>(maxLength: 500, nullable: true),
                    LanguageId = table.Column<string>(maxLength: 32, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Resources", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Languages_Code",
                schema: "Localization",
                table: "Languages",
                column: "Code",
                unique: true,
                filter: "[Code] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Resources_LanguageId_Key",
                schema: "Localization",
                table: "Resources",
                columns: new[] { "LanguageId", "Key" },
                unique: true,
                filter: "[LanguageId] IS NOT NULL AND [Key] IS NOT NULL");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Languages",
                schema: "Localization");

            migrationBuilder.DropTable(
                name: "Resources",
                schema: "Localization");
        }
    }
}
