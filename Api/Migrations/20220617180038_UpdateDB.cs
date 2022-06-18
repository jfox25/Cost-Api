using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Api.Migrations
{
    public partial class UpdateDB : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "FrequentId",
                table: "Expenses",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "AspNetUsers",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastActive",
                table: "AspNetUsers",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.CreateIndex(
                name: "IX_Expenses_FrequentId",
                table: "Expenses",
                column: "FrequentId");

            migrationBuilder.AddForeignKey(
                name: "FK_Expenses_Frequents_FrequentId",
                table: "Expenses",
                column: "FrequentId",
                principalTable: "Frequents",
                principalColumn: "FrequentId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Expenses_Frequents_FrequentId",
                table: "Expenses");

            migrationBuilder.DropIndex(
                name: "IX_Expenses_FrequentId",
                table: "Expenses");

            migrationBuilder.DropColumn(
                name: "FrequentId",
                table: "Expenses");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "LastActive",
                table: "AspNetUsers");
        }
    }
}
