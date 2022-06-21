using Microsoft.EntityFrameworkCore.Migrations;

namespace Api.Migrations
{
    public partial class LookupAnalyticUpdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "LookupTypeName",
                table: "LookupCounts",
                type: "longtext CHARACTER SET utf8mb4",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "LookupTypeId",
                table: "LookupAnalytics",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "LookupTypeName",
                table: "LookupAnalytics",
                type: "longtext CHARACTER SET utf8mb4",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LookupTypeName",
                table: "LookupCounts");

            migrationBuilder.DropColumn(
                name: "LookupTypeId",
                table: "LookupAnalytics");

            migrationBuilder.DropColumn(
                name: "LookupTypeName",
                table: "LookupAnalytics");

        }
    }
}
