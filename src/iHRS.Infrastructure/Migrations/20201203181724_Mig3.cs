using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace iHRS.Infrastructure.Migrations
{
    public partial class Mig3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Employees",
                keyColumn: "EmployeeId",
                keyValue: new Guid("00000000-0000-0000-0000-000000000002"),
                column: "PasswordChanged",
                value: true);

            migrationBuilder.UpdateData(
                table: "Employees",
                keyColumn: "EmployeeId",
                keyValue: new Guid("00000000-0000-0000-0000-000000000003"),
                column: "PasswordChanged",
                value: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Employees",
                keyColumn: "EmployeeId",
                keyValue: new Guid("00000000-0000-0000-0000-000000000002"),
                column: "PasswordChanged",
                value: false);

            migrationBuilder.UpdateData(
                table: "Employees",
                keyColumn: "EmployeeId",
                keyValue: new Guid("00000000-0000-0000-0000-000000000003"),
                column: "PasswordChanged",
                value: false);
        }
    }
}
