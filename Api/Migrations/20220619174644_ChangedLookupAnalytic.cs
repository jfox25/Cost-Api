using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Api.Migrations
{
    public partial class ChangedLookupAnalytic : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CategoryCounts");

            migrationBuilder.DropTable(
                name: "DirectiveCounts");

            migrationBuilder.DropTable(
                name: "LocationCounts");

            migrationBuilder.DropTable(
                name: "CategoryAnalytics");

            migrationBuilder.DropTable(
                name: "DirectiveAnalytics");

            migrationBuilder.DropTable(
                name: "LocationAnalytics");

            migrationBuilder.CreateTable(
                name: "LookupAnalytics",
                columns: table => new
                {
                    LookupAnalyticId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Date = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    UserId = table.Column<string>(type: "varchar(255) CHARACTER SET utf8mb4", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LookupAnalytics", x => x.LookupAnalyticId);
                    table.ForeignKey(
                        name: "FK_LookupAnalytics_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "LookupTypes",
                columns: table => new
                {
                    LookupTypeId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    LookupName = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LookupTypes", x => x.LookupTypeId);
                });

            migrationBuilder.CreateTable(
                name: "LookupCounts",
                columns: table => new
                {
                    LookupCountId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    LookupId = table.Column<int>(type: "int", nullable: false),
                    LookupTypeId = table.Column<int>(type: "int", nullable: false),
                    NumberOfExpenses = table.Column<int>(type: "int", nullable: false),
                    TotalCostOfExpenses = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<string>(type: "varchar(255) CHARACTER SET utf8mb4", nullable: true),
                    LookupAnalyticId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LookupCounts", x => x.LookupCountId);
                    table.ForeignKey(
                        name: "FK_LookupCounts_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_LookupCounts_LookupAnalytics_LookupAnalyticId",
                        column: x => x.LookupAnalyticId,
                        principalTable: "LookupAnalytics",
                        principalColumn: "LookupAnalyticId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_LookupCounts_LookupTypes_LookupTypeId",
                        column: x => x.LookupTypeId,
                        principalTable: "LookupTypes",
                        principalColumn: "LookupTypeId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_LookupAnalytics_UserId",
                table: "LookupAnalytics",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_LookupCounts_LookupAnalyticId",
                table: "LookupCounts",
                column: "LookupAnalyticId");

            migrationBuilder.CreateIndex(
                name: "IX_LookupCounts_LookupTypeId",
                table: "LookupCounts",
                column: "LookupTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_LookupCounts_UserId",
                table: "LookupCounts",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LookupCounts");

            migrationBuilder.DropTable(
                name: "LookupAnalytics");

            migrationBuilder.DropTable(
                name: "LookupTypes");

            migrationBuilder.CreateTable(
                name: "CategoryAnalytics",
                columns: table => new
                {
                    CategoryAnalyticId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Date = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    UserId = table.Column<string>(type: "varchar(255) CHARACTER SET utf8mb4", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CategoryAnalytics", x => x.CategoryAnalyticId);
                    table.ForeignKey(
                        name: "FK_CategoryAnalytics_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "DirectiveAnalytics",
                columns: table => new
                {
                    DirectiveAnalyticId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Date = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    UserId = table.Column<string>(type: "varchar(255) CHARACTER SET utf8mb4", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DirectiveAnalytics", x => x.DirectiveAnalyticId);
                    table.ForeignKey(
                        name: "FK_DirectiveAnalytics_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "LocationAnalytics",
                columns: table => new
                {
                    LocationAnalyticId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Date = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    UserId = table.Column<string>(type: "varchar(255) CHARACTER SET utf8mb4", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LocationAnalytics", x => x.LocationAnalyticId);
                    table.ForeignKey(
                        name: "FK_LocationAnalytics_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "CategoryCounts",
                columns: table => new
                {
                    CategoryCountId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CategoryAnalyticId = table.Column<int>(type: "int", nullable: true),
                    CategoryId = table.Column<int>(type: "int", nullable: false),
                    Date = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    NumberOfExpenses = table.Column<int>(type: "int", nullable: false),
                    TotalCostOfExpenses = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<string>(type: "varchar(255) CHARACTER SET utf8mb4", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CategoryCounts", x => x.CategoryCountId);
                    table.ForeignKey(
                        name: "FK_CategoryCounts_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CategoryCounts_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Categories",
                        principalColumn: "CategoryId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CategoryCounts_CategoryAnalytics_CategoryAnalyticId",
                        column: x => x.CategoryAnalyticId,
                        principalTable: "CategoryAnalytics",
                        principalColumn: "CategoryAnalyticId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "DirectiveCounts",
                columns: table => new
                {
                    DirectiveCountId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Date = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    DirectiveAnalyticId = table.Column<int>(type: "int", nullable: true),
                    DirectiveId = table.Column<int>(type: "int", nullable: false),
                    NumberOfExpenses = table.Column<int>(type: "int", nullable: false),
                    TotalCostOfExpenses = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<string>(type: "varchar(255) CHARACTER SET utf8mb4", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DirectiveCounts", x => x.DirectiveCountId);
                    table.ForeignKey(
                        name: "FK_DirectiveCounts_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DirectiveCounts_DirectiveAnalytics_DirectiveAnalyticId",
                        column: x => x.DirectiveAnalyticId,
                        principalTable: "DirectiveAnalytics",
                        principalColumn: "DirectiveAnalyticId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DirectiveCounts_Directives_DirectiveId",
                        column: x => x.DirectiveId,
                        principalTable: "Directives",
                        principalColumn: "DirectiveId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "LocationCounts",
                columns: table => new
                {
                    LocationCountId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Date = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    LocationAnalyticId = table.Column<int>(type: "int", nullable: true),
                    LocationId = table.Column<int>(type: "int", nullable: false),
                    NumberOfExpenses = table.Column<int>(type: "int", nullable: false),
                    TotalCostOfExpenses = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<string>(type: "varchar(255) CHARACTER SET utf8mb4", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LocationCounts", x => x.LocationCountId);
                    table.ForeignKey(
                        name: "FK_LocationCounts_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_LocationCounts_LocationAnalytics_LocationAnalyticId",
                        column: x => x.LocationAnalyticId,
                        principalTable: "LocationAnalytics",
                        principalColumn: "LocationAnalyticId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_LocationCounts_Locations_LocationId",
                        column: x => x.LocationId,
                        principalTable: "Locations",
                        principalColumn: "LocationId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CategoryAnalytics_UserId",
                table: "CategoryAnalytics",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_CategoryCounts_CategoryAnalyticId",
                table: "CategoryCounts",
                column: "CategoryAnalyticId");

            migrationBuilder.CreateIndex(
                name: "IX_CategoryCounts_CategoryId",
                table: "CategoryCounts",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_CategoryCounts_UserId",
                table: "CategoryCounts",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_DirectiveAnalytics_UserId",
                table: "DirectiveAnalytics",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_DirectiveCounts_DirectiveAnalyticId",
                table: "DirectiveCounts",
                column: "DirectiveAnalyticId");

            migrationBuilder.CreateIndex(
                name: "IX_DirectiveCounts_DirectiveId",
                table: "DirectiveCounts",
                column: "DirectiveId");

            migrationBuilder.CreateIndex(
                name: "IX_DirectiveCounts_UserId",
                table: "DirectiveCounts",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_LocationAnalytics_UserId",
                table: "LocationAnalytics",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_LocationCounts_LocationAnalyticId",
                table: "LocationCounts",
                column: "LocationAnalyticId");

            migrationBuilder.CreateIndex(
                name: "IX_LocationCounts_LocationId",
                table: "LocationCounts",
                column: "LocationId");

            migrationBuilder.CreateIndex(
                name: "IX_LocationCounts_UserId",
                table: "LocationCounts",
                column: "UserId");
        }
    }
}
