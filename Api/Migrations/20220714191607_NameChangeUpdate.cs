using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Api.Migrations
{
    public partial class NameChangeUpdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Expenses_Locations_LocationId",
                table: "Expenses");

            migrationBuilder.DropForeignKey(
                name: "FK_Frequents_Locations_LocationId",
                table: "Frequents");

            migrationBuilder.DropTable(
                name: "Locations");

            migrationBuilder.RenameColumn(
                name: "MostExpensiveLocationId",
                table: "GeneralAnalytics",
                newName: "MostExpensiveBusinessId");

            migrationBuilder.RenameColumn(
                name: "LocationName",
                table: "GeneralAnalytics",
                newName: "BusinessName");

            migrationBuilder.RenameColumn(
                name: "LocationId",
                table: "Frequents",
                newName: "BusinessId");

            migrationBuilder.RenameIndex(
                name: "IX_Frequents_LocationId",
                table: "Frequents",
                newName: "IX_Frequents_BusinessId");

            migrationBuilder.RenameColumn(
                name: "LocationId",
                table: "Expenses",
                newName: "BusinessId");

            migrationBuilder.RenameIndex(
                name: "IX_Expenses_LocationId",
                table: "Expenses",
                newName: "IX_Expenses_BusinessId");

            migrationBuilder.RenameColumn(
                name: "MostExpensiveLocationId",
                table: "CurrentDataAnalytics",
                newName: "MostExpensiveBusinessId");

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "Frequents",
                type: "longtext CHARACTER SET utf8mb4",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Businesses",
                columns: table => new
                {
                    BusinessId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: true),
                    CategoryId = table.Column<int>(type: "int", nullable: false),
                    CategoryName = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: true),
                    UserId = table.Column<string>(type: "varchar(255) CHARACTER SET utf8mb4", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Businesses", x => x.BusinessId);
                    table.ForeignKey(
                        name: "FK_Businesses_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Businesses_UserId",
                table: "Businesses",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Expenses_Businesses_BusinessId",
                table: "Expenses",
                column: "BusinessId",
                principalTable: "Businesses",
                principalColumn: "BusinessId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Frequents_Businesses_BusinessId",
                table: "Frequents",
                column: "BusinessId",
                principalTable: "Businesses",
                principalColumn: "BusinessId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Expenses_Businesses_BusinessId",
                table: "Expenses");

            migrationBuilder.DropForeignKey(
                name: "FK_Frequents_Businesses_BusinessId",
                table: "Frequents");

            migrationBuilder.DropTable(
                name: "Businesses");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "Frequents");

            migrationBuilder.RenameColumn(
                name: "MostExpensiveBusinessId",
                table: "GeneralAnalytics",
                newName: "MostExpensiveLocationId");

            migrationBuilder.RenameColumn(
                name: "BusinessName",
                table: "GeneralAnalytics",
                newName: "LocationName");

            migrationBuilder.RenameColumn(
                name: "BusinessId",
                table: "Frequents",
                newName: "LocationId");

            migrationBuilder.RenameIndex(
                name: "IX_Frequents_BusinessId",
                table: "Frequents",
                newName: "IX_Frequents_LocationId");

            migrationBuilder.RenameColumn(
                name: "BusinessId",
                table: "Expenses",
                newName: "LocationId");

            migrationBuilder.RenameIndex(
                name: "IX_Expenses_BusinessId",
                table: "Expenses",
                newName: "IX_Expenses_LocationId");

            migrationBuilder.RenameColumn(
                name: "MostExpensiveBusinessId",
                table: "CurrentDataAnalytics",
                newName: "MostExpensiveLocationId");

            migrationBuilder.CreateTable(
                name: "Locations",
                columns: table => new
                {
                    LocationId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CategoryId = table.Column<int>(type: "int", nullable: false),
                    CategoryName = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: true),
                    Name = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: true),
                    UserId = table.Column<string>(type: "varchar(255) CHARACTER SET utf8mb4", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Locations", x => x.LocationId);
                    table.ForeignKey(
                        name: "FK_Locations_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Locations_UserId",
                table: "Locations",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Expenses_Locations_LocationId",
                table: "Expenses",
                column: "LocationId",
                principalTable: "Locations",
                principalColumn: "LocationId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Frequents_Locations_LocationId",
                table: "Frequents",
                column: "LocationId",
                principalTable: "Locations",
                principalColumn: "LocationId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
