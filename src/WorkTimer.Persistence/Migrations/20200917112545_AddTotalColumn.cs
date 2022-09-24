using Microsoft.EntityFrameworkCore.Migrations;

namespace WorkTimer.Persistence.Migrations;

public partial class AddTotalColumn : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.AddColumn<double>(
            name: "TotalHours",
            table: "WorkDays",
            type: "REAL",
            nullable: false,
            defaultValue: 0.0);

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
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropColumn(
            name: "TotalHours",
            table: "WorkDays");

        migrationBuilder.UpdateData(
            table: "AspNetRoles",
            keyColumn: "Id",
            keyValue: 1,
            column: "ConcurrencyStamp",
            value: "b3234b04-e72b-4be0-8d17-5123069ad41f");

        migrationBuilder.UpdateData(
            table: "AspNetRoles",
            keyColumn: "Id",
            keyValue: 2,
            column: "ConcurrencyStamp",
            value: "c74fb88d-4c00-4680-b9d8-b42cec39e59f");
    }
}
