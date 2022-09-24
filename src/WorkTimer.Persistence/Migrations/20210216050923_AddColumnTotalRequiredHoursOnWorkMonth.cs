using Microsoft.EntityFrameworkCore.Migrations;

namespace WorkTimer.Persistence.Migrations;

public partial class AddColumnTotalRequiredHoursOnWorkMonth : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.AddColumn<double>(
            name: "TotalRequiredHours",
            table: "WorkWeeks",
            type: "REAL",
            nullable: false,
            defaultValue: 0.0);

        migrationBuilder.AddColumn<double>(
            name: "TotalRequiredHours",
            table: "WorkMonths",
            type: "REAL",
            nullable: false,
            defaultValue: 0.0);

        migrationBuilder.UpdateData(
            table: "AspNetRoles",
            keyColumn: "Id",
            keyValue: 1,
            column: "ConcurrencyStamp",
            value: "9b8eb2ce-088b-4905-90cb-c19597d4c167");

        migrationBuilder.UpdateData(
            table: "AspNetRoles",
            keyColumn: "Id",
            keyValue: 2,
            column: "ConcurrencyStamp",
            value: "4d5bd4c3-3875-4787-b7e6-2a904c185560");
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropColumn(
            name: "TotalRequiredHours",
            table: "WorkWeeks");

        migrationBuilder.DropColumn(
            name: "TotalRequiredHours",
            table: "WorkMonths");

        migrationBuilder.UpdateData(
            table: "AspNetRoles",
            keyColumn: "Id",
            keyValue: 1,
            column: "ConcurrencyStamp",
            value: "8cd53aaf-8228-4747-b216-d3d45c17cc65");

        migrationBuilder.UpdateData(
            table: "AspNetRoles",
            keyColumn: "Id",
            keyValue: 2,
            column: "ConcurrencyStamp",
            value: "43cbc523-5281-4e2b-9c25-188d146be0ae");
    }
}
