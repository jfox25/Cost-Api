using Microsoft.EntityFrameworkCore.Migrations;

namespace Api.Migrations
{
    public partial class FrequentUpdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "LastBilledDate",
                table: "Frequents",
                newName: "LastUsedDate");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "LastUsedDate",
                table: "Frequents",
                newName: "LastBilledDate");
        }
    }
}
