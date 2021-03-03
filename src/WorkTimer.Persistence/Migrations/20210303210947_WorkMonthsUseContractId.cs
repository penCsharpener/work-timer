using Microsoft.EntityFrameworkCore.Migrations;

namespace WorkTimer.Persistence.Migrations
{
    public partial class WorkMonthsUseContractId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WorkWeeks_AspNetUsers_UserId",
                table: "WorkWeeks");

            migrationBuilder.DropIndex(
                name: "IX_WorkWeeks_UserId_WeekStart",
                table: "WorkWeeks");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "WorkMonths",
                newName: "ContractId");

            migrationBuilder.RenameIndex(
                name: "IX_WorkMonths_UserId_Month_Year",
                table: "WorkMonths",
                newName: "IX_WorkMonths_ContractId_Month_Year");

            migrationBuilder.AlterColumn<int>(
                name: "UserId",
                table: "WorkWeeks",
                type: "INTEGER",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "INTEGER");

            migrationBuilder.AddColumn<int>(
                name: "ContractId",
                table: "WorkWeeks",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 1,
                column: "ConcurrencyStamp",
                value: "420fdcb2-4d49-46aa-ab64-53624a6a3170");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 2,
                column: "ConcurrencyStamp",
                value: "35ba00f0-a871-439a-b616-8ab057ca3178");

            migrationBuilder.CreateIndex(
                name: "IX_WorkWeeks_ContractId_WeekStart",
                table: "WorkWeeks",
                columns: new[] { "ContractId", "WeekStart" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_WorkWeeks_UserId",
                table: "WorkWeeks",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_WorkMonths_Contracts_ContractId",
                table: "WorkMonths",
                column: "ContractId",
                principalTable: "Contracts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_WorkWeeks_AspNetUsers_UserId",
                table: "WorkWeeks",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_WorkWeeks_Contracts_ContractId",
                table: "WorkWeeks",
                column: "ContractId",
                principalTable: "Contracts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WorkMonths_Contracts_ContractId",
                table: "WorkMonths");

            migrationBuilder.DropForeignKey(
                name: "FK_WorkWeeks_AspNetUsers_UserId",
                table: "WorkWeeks");

            migrationBuilder.DropForeignKey(
                name: "FK_WorkWeeks_Contracts_ContractId",
                table: "WorkWeeks");

            migrationBuilder.DropIndex(
                name: "IX_WorkWeeks_ContractId_WeekStart",
                table: "WorkWeeks");

            migrationBuilder.DropIndex(
                name: "IX_WorkWeeks_UserId",
                table: "WorkWeeks");

            migrationBuilder.DropColumn(
                name: "ContractId",
                table: "WorkWeeks");

            migrationBuilder.RenameColumn(
                name: "ContractId",
                table: "WorkMonths",
                newName: "UserId");

            migrationBuilder.RenameIndex(
                name: "IX_WorkMonths_ContractId_Month_Year",
                table: "WorkMonths",
                newName: "IX_WorkMonths_UserId_Month_Year");

            migrationBuilder.AlterColumn<int>(
                name: "UserId",
                table: "WorkWeeks",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "INTEGER",
                oldNullable: true);

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

            migrationBuilder.CreateIndex(
                name: "IX_WorkWeeks_UserId_WeekStart",
                table: "WorkWeeks",
                columns: new[] { "UserId", "WeekStart" },
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_WorkWeeks_AspNetUsers_UserId",
                table: "WorkWeeks",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
