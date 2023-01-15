using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WorkTimer.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddTodoIsCompleted : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedOn",
                table: "Todos",
                type: "TEXT",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "TEXT",
                oldDefaultValue: new DateTime(2022, 10, 23, 17, 0, 11, 566, DateTimeKind.Local).AddTicks(2737));

            migrationBuilder.AddColumn<bool>(
                name: "IsCompleted",
                table: "Todos",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedOn",
                table: "Notes",
                type: "TEXT",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "TEXT",
                oldDefaultValue: new DateTime(2022, 10, 23, 17, 0, 11, 565, DateTimeKind.Local).AddTicks(7377));

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 1,
                column: "ConcurrencyStamp",
                value: "6eca6d3e-9ea1-4a70-96f8-38ae8eeec304");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 2,
                column: "ConcurrencyStamp",
                value: "a2a43b05-4eb2-4f8e-863a-a0e4bd2c7a09");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsCompleted",
                table: "Todos");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedOn",
                table: "Todos",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateTime(2022, 10, 23, 17, 0, 11, 566, DateTimeKind.Local).AddTicks(2737),
                oldClrType: typeof(DateTime),
                oldType: "TEXT");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedOn",
                table: "Notes",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateTime(2022, 10, 23, 17, 0, 11, 565, DateTimeKind.Local).AddTicks(7377),
                oldClrType: typeof(DateTime),
                oldType: "TEXT");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 1,
                column: "ConcurrencyStamp",
                value: "cd40199e-54cd-4717-8901-e3f3efefbf59");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 2,
                column: "ConcurrencyStamp",
                value: "7cdbc6c1-ca8c-4b83-ac45-9101ac88661f");
        }
    }
}
