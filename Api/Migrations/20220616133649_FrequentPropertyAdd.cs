using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Api.Migrations
{
    public partial class FrequentPropertyAdd : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "LastBilledDate",
                table: "Frequents",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LastBilledDate",
                table: "Frequents");
        }
    }
}
