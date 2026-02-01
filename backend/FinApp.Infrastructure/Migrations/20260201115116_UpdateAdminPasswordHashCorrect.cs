using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FinApp.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateAdminPasswordHashCorrect : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000010"),
                column: "UpdatedAt",
                value: new DateTime(2026, 2, 1, 11, 51, 16, 72, DateTimeKind.Utc).AddTicks(9262));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000011"),
                column: "UpdatedAt",
                value: new DateTime(2026, 2, 1, 11, 51, 16, 72, DateTimeKind.Utc).AddTicks(9271));

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000001"),
                column: "PasswordHash",
                value: "$2a$11$VyFHd84rlCboUP.RPn25qeR7gw9i39bjj65fARIUvG6JkSjv.E2mW");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000010"),
                column: "UpdatedAt",
                value: new DateTime(2026, 2, 1, 11, 46, 18, 59, DateTimeKind.Utc).AddTicks(8499));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000011"),
                column: "UpdatedAt",
                value: new DateTime(2026, 2, 1, 11, 46, 18, 59, DateTimeKind.Utc).AddTicks(8508));

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000001"),
                column: "PasswordHash",
                value: "$2a$11$6mtygzX7D/O53nh87B5W3O3ro/wBXjAF64kFyYrthx5vpWsg9vfmO");
        }
    }
}
