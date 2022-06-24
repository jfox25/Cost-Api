using Microsoft.EntityFrameworkCore.Migrations;

namespace Api.Migrations
{
    public partial class AnalyticObjectChanges : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "LookupName",
                table: "LookupCounts",
                type: "longtext CHARACTER SET utf8mb4",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CategoryName",
                table: "GeneralAnalytics",
                type: "longtext CHARACTER SET utf8mb4",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DirectiveName",
                table: "GeneralAnalytics",
                type: "longtext CHARACTER SET utf8mb4",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LocationName",
                table: "GeneralAnalytics",
                type: "longtext CHARACTER SET utf8mb4",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LookupName",
                table: "LookupCounts");

            migrationBuilder.DropColumn(
                name: "CategoryName",
                table: "GeneralAnalytics");

            migrationBuilder.DropColumn(
                name: "DirectiveName",
                table: "GeneralAnalytics");

            migrationBuilder.DropColumn(
                name: "LocationName",
                table: "GeneralAnalytics");
        }
    }
}
