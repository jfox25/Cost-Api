using Microsoft.EntityFrameworkCore.Migrations;

namespace Api.Migrations
{
    public partial class DirectiveIdChange : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "Directives",
                type: "varchar(255) CHARACTER SET utf8mb4",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Directives_UserId",
                table: "Directives",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Directives_AspNetUsers_UserId",
                table: "Directives",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Directives_AspNetUsers_UserId",
                table: "Directives");

            migrationBuilder.DropIndex(
                name: "IX_Directives_UserId",
                table: "Directives");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Directives");
        }
    }
}
