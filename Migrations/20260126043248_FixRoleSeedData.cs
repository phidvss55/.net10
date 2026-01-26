using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace webapi.Migrations
{
    /// <inheritdoc />
    public partial class FixRoleSeedData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "083f5396-b577-46b3-b9e1-a0ce2b4c1b01");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "455976f1-736a-4e39-96f7-59b1b74765f6");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "a1b2c3d4-e5f6-7890-abcd-ef1234567890", "db483d24-7f46-45fc-9beb-fc545399e3c0", "Admin", "ADMIN" },
                    { "b2c3d4e5-f6a7-8901-bcde-f12345678901", "014cfc82-5ee4-45b6-8a2d-6c37be125686", "User", "USER" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "a1b2c3d4-e5f6-7890-abcd-ef1234567890");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "b2c3d4e5-f6a7-8901-bcde-f12345678901");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "083f5396-b577-46b3-b9e1-a0ce2b4c1b01", "e4c7017c-aa8d-4297-9050-545eefaf521e", "User", "USER" },
                    { "455976f1-736a-4e39-96f7-59b1b74765f6", "3f1da3ab-bb4f-4e3d-9416-f7d560a4bc73", "Admin", "ADMIN" }
                });
        }
    }
}
