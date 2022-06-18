using Microsoft.EntityFrameworkCore.Migrations;

namespace Api.Migrations
{
    public partial class ApplicationUserPropUpdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsDeadUser",
                table: "AspNetUsers",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsDeadUser",
                table: "AspNetUsers");
        }
    }
}
