using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SFood.DataAccess.Migrator.Migrations
{
    public partial class AddSeatAreaAndCenterBanner : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Images_BasicInfos_CenterId",
                schema: "Common",
                table: "Images");

            migrationBuilder.DropForeignKey(
                name: "FK_Bills_Archives_OrderId",
                schema: "OrderInfo",
                table: "Bills");

            migrationBuilder.DropIndex(
                name: "IX_Images_CenterId",
                schema: "Common",
                table: "Images");

            migrationBuilder.DropColumn(
                name: "CenterId",
                schema: "Common",
                table: "Images");

            migrationBuilder.AddColumn<string>(
                name: "SeatAreaId",
                schema: "HawkerCenter",
                table: "Seats",
                maxLength: 32,
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Banner",
                schema: "HawkerCenter",
                columns: table => new
                {
                    Id = table.Column<string>(maxLength: 32, nullable: false),
                    CenterId = table.Column<string>(nullable: true),
                    TargetUrl = table.Column<string>(nullable: true),
                    StartAt = table.Column<DateTime>(nullable: false),
                    EndAt = table.Column<DateTime>(nullable: false),
                    Visit = table.Column<int>(nullable: false),
                    CreatedTime = table.Column<DateTime>(nullable: false, defaultValueSql: "getutcdate()"),
                    CreatedUserName = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Banner", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SeatAreas",
                schema: "HawkerCenter",
                columns: table => new
                {
                    Id = table.Column<string>(maxLength: 32, nullable: false),
                    CenterId = table.Column<string>(maxLength: 32, nullable: true),
                    Name = table.Column<string>(maxLength: 32, nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    CreatedTime = table.Column<DateTime>(nullable: false, defaultValueSql: "getutcdate()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SeatAreas", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Banner_CenterId",
                schema: "HawkerCenter",
                table: "Banner",
                column: "CenterId");

            migrationBuilder.CreateIndex(
                name: "IX_SeatAreas_CenterId",
                schema: "HawkerCenter",
                table: "SeatAreas",
                column: "CenterId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Banner",
                schema: "HawkerCenter");

            migrationBuilder.DropTable(
                name: "SeatAreas",
                schema: "HawkerCenter");

            migrationBuilder.DropColumn(
                name: "SeatAreaId",
                schema: "HawkerCenter",
                table: "Seats");

            migrationBuilder.AddColumn<string>(
                name: "CenterId",
                schema: "Common",
                table: "Images",
                maxLength: 32,
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Images_CenterId",
                schema: "Common",
                table: "Images",
                column: "CenterId");

            migrationBuilder.AddForeignKey(
                name: "FK_Images_BasicInfos_CenterId",
                schema: "Common",
                table: "Images",
                column: "CenterId",
                principalSchema: "HawkerCenter",
                principalTable: "BasicInfos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Bills_Archives_OrderId",
                schema: "OrderInfo",
                table: "Bills",
                column: "OrderId",
                principalSchema: "OrderInfo",
                principalTable: "Archives",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
