using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace iHRS.Infrastructure.Migrations
{
    public partial class Mig5 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Hotels",
                keyColumn: "HotelId",
                keyValue: new Guid("405c2ed5-f2f1-4170-8717-4117a8ae8133"));

            migrationBuilder.InsertData(
                table: "Hotels",
                columns: new[] { "HotelId", "CreatedBy", "CreatedOn", "ExpirationDate", "ModifiedBy", "ModifiedOn", "Name", "TenantId" },
                values: new object[] { new Guid("00000000-0000-0000-0000-000000000004"), "System", new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "System", new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "My first hotel", new Guid("00000000-0000-0000-0000-000000000001") });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Hotels",
                keyColumn: "HotelId",
                keyValue: new Guid("00000000-0000-0000-0000-000000000004"));

            migrationBuilder.InsertData(
                table: "Hotels",
                columns: new[] { "HotelId", "CreatedBy", "CreatedOn", "ExpirationDate", "ModifiedBy", "ModifiedOn", "Name", "TenantId" },
                values: new object[] { new Guid("405c2ed5-f2f1-4170-8717-4117a8ae8133"), "System", new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "System", new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "My first hotel", new Guid("00000000-0000-0000-0000-000000000001") });
        }
    }
}
