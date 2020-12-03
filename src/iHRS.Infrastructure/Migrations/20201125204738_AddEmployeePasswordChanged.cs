using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace iHRS.Infrastructure.Migrations
{
    public partial class AddEmployeePasswordChanged : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "PasswordChanged",
                table: "Employees",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.InsertData(
                table: "Employees",
                columns: new[] { "EmployeeId", "CreatedBy", "CreatedOn", "DateOfBirth", "EmailAddress", "ExpirationDate", "FirstName", "LastName", "ModifiedBy", "ModifiedOn", "Password", "PasswordChanged", "RoleId", "TenantId" },
                values: new object[] { new Guid("00000000-0000-0000-0000-000000000003"), "System", new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(1995, 4, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), "user2@example.com", null, "Ewa", "Kowalska", "System", new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "AQAAAAEAACcQAAAAENU6ixP+jXYINKxOpVeXbTl0X9q83k4cUIXSMPv0iQZro7F2xMN7t7otCg1O3IueJQ==", false, 2, new Guid("00000000-0000-0000-0000-000000000001") });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Employees",
                keyColumn: "EmployeeId",
                keyValue: new Guid("00000000-0000-0000-0000-000000000003"));

            migrationBuilder.DropColumn(
                name: "PasswordChanged",
                table: "Employees");
        }
    }
}
