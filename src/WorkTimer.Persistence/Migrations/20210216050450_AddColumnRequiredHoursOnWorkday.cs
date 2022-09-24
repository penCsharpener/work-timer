using Microsoft.EntityFrameworkCore.Migrations;

namespace WorkTimer.Persistence.Migrations;

public partial class AddColumnRequiredHoursOnWorkday : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.AddColumn<double>(
            name: "RequiredHours",
            table: "WorkDays",
            type: "REAL",
            nullable: false,
            defaultValue: 0.0);

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

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropColumn(
            name: "RequiredHours",
            table: "WorkDays");

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
    }
}
