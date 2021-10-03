using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace WorkTimer.Persistence.Migrations
{
    public partial class AddWorkMonthsAndWeeks : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_WorkDays_ContractId",
                table: "WorkDays");

            migrationBuilder.AddColumn<int>(
                name: "WorkMonthId",
                table: "WorkDays",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "WorkWeekId",
                table: "WorkDays",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "WorkMonths",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    UserId = table.Column<int>(type: "INTEGER", nullable: false),
                    DaysWorked = table.Column<int>(type: "INTEGER", nullable: false),
                    DaysOffWork = table.Column<int>(type: "INTEGER", nullable: false),
                    TotalOverhours = table.Column<double>(type: "REAL", nullable: false),
                    TotalHours = table.Column<double>(type: "REAL", nullable: false),
                    Year = table.Column<int>(type: "INTEGER", nullable: false),
                    Month = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkMonths", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "WorkWeeks",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    UserId = table.Column<int>(type: "INTEGER", nullable: false),
                    DaysWorked = table.Column<int>(type: "INTEGER", nullable: false),
                    DaysOffWork = table.Column<int>(type: "INTEGER", nullable: false),
                    TotalOverhours = table.Column<double>(type: "REAL", nullable: false),
                    TotalHours = table.Column<double>(type: "REAL", nullable: false),
                    WeekNumber = table.Column<int>(type: "INTEGER", nullable: false),
                    WeekStart = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkWeeks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WorkWeeks_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 1,
                column: "ConcurrencyStamp",
                value: "9aa9e378-bc58-47f9-909e-8a379dd90fd4");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 2,
                column: "ConcurrencyStamp",
                value: "9d12dc7f-4c92-46a7-b8bb-a80b75deb2a7");

            migrationBuilder.CreateIndex(
                name: "IX_WorkDays_ContractId_Date",
                table: "WorkDays",
                columns: new[] { "ContractId", "Date" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_WorkDays_WorkMonthId",
                table: "WorkDays",
                column: "WorkMonthId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkDays_WorkWeekId",
                table: "WorkDays",
                column: "WorkWeekId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkMonths_UserId_Month_Year",
                table: "WorkMonths",
                columns: new[] { "UserId", "Month", "Year" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_WorkWeeks_UserId_WeekStart",
                table: "WorkWeeks",
                columns: new[] { "UserId", "WeekStart" },
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_WorkDays_WorkMonths_WorkMonthId",
                table: "WorkDays",
                column: "WorkMonthId",
                principalTable: "WorkMonths",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_WorkDays_WorkWeeks_WorkWeekId",
                table: "WorkDays",
                column: "WorkWeekId",
                principalTable: "WorkWeeks",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WorkDays_WorkMonths_WorkMonthId",
                table: "WorkDays");

            migrationBuilder.DropForeignKey(
                name: "FK_WorkDays_WorkWeeks_WorkWeekId",
                table: "WorkDays");

            migrationBuilder.DropTable(
                name: "WorkMonths");

            migrationBuilder.DropTable(
                name: "WorkWeeks");

            migrationBuilder.DropIndex(
                name: "IX_WorkDays_ContractId_Date",
                table: "WorkDays");

            migrationBuilder.DropIndex(
                name: "IX_WorkDays_WorkMonthId",
                table: "WorkDays");

            migrationBuilder.DropIndex(
                name: "IX_WorkDays_WorkWeekId",
                table: "WorkDays");

            migrationBuilder.DropColumn(
                name: "WorkMonthId",
                table: "WorkDays");

            migrationBuilder.DropColumn(
                name: "WorkWeekId",
                table: "WorkDays");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 1,
                column: "ConcurrencyStamp",
                value: "fa62af81-dc13-458a-8e8d-6ca2123d4cb0");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 2,
                column: "ConcurrencyStamp",
                value: "a8d8a0b8-a52b-4b7d-b5cc-e9122c380fab");

            migrationBuilder.CreateIndex(
                name: "IX_WorkDays_ContractId",
                table: "WorkDays",
                column: "ContractId");
        }
    }
}
