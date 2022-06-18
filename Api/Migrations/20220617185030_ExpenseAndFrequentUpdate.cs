using Microsoft.EntityFrameworkCore.Migrations;

namespace Api.Migrations
{
    public partial class ExpenseAndFrequentUpdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Expenses_Frequents_FrequentId",
                table: "Expenses");

            migrationBuilder.DropIndex(
                name: "IX_Expenses_FrequentId",
                table: "Expenses");

            migrationBuilder.AddColumn<bool>(
                name: "IsRecurringExpense",
                table: "Expenses",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsRecurringExpense",
                table: "Expenses");

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
    }
}
