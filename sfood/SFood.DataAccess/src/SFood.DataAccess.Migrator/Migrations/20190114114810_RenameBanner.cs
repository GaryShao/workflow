using Microsoft.EntityFrameworkCore.Migrations;

namespace SFood.DataAccess.Migrator.Migrations
{
    public partial class RenameBanner : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Banner",
                schema: "HawkerCenter",
                table: "Banner");

            migrationBuilder.RenameTable(
                name: "Banner",
                schema: "HawkerCenter",
                newName: "Banners",
                newSchema: "HawkerCenter");

            migrationBuilder.RenameIndex(
                name: "IX_Banner_CenterId",
                schema: "HawkerCenter",
                table: "Banners",
                newName: "IX_Banners_CenterId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Banners",
                schema: "HawkerCenter",
                table: "Banners",
                column: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Banners",
                schema: "HawkerCenter",
                table: "Banners");

            migrationBuilder.RenameTable(
                name: "Banners",
                schema: "HawkerCenter",
                newName: "Banner",
                newSchema: "HawkerCenter");

            migrationBuilder.RenameIndex(
                name: "IX_Banners_CenterId",
                schema: "HawkerCenter",
                table: "Banner",
                newName: "IX_Banner_CenterId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Banner",
                schema: "HawkerCenter",
                table: "Banner",
                column: "Id");
        }
    }
}
