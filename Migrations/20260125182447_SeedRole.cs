using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace webapi.Migrations
{
    /// <inheritdoc />
    public partial class SeedRole : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "083f5396-b577-46b3-b9e1-a0ce2b4c1b01", "e4c7017c-aa8d-4297-9050-545eefaf521e", "User", "USER" },
                    { "455976f1-736a-4e39-96f7-59b1b74765f6", "3f1da3ab-bb4f-4e3d-9416-f7d560a4bc73", "Admin", "ADMIN" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "083f5396-b577-46b3-b9e1-a0ce2b4c1b01");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "455976f1-736a-4e39-96f7-59b1b74765f6");
        }
    }
}
