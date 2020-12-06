using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace iHRS.Infrastructure.Migrations
{
    public partial class Mig7 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Hotels",
                columns: new[] { "HotelId", "CreatedBy", "CreatedOn", "ExpirationDate", "ModifiedBy", "ModifiedOn", "Name", "TenantId" },
                values: new object[] { new Guid("00000000-0000-0000-0000-000000000005"), "System", new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "System", new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "My second hotel", new Guid("00000000-0000-0000-0000-000000000001") });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Hotels",
                keyColumn: "HotelId",
                keyValue: new Guid("00000000-0000-0000-0000-000000000005"));
        }
    }
}
