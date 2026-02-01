using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FinApp.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class FixRoleSuboutputsUpdatedAt : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000010"),
                column: "UpdatedAt",
                value: new DateTime(2026, 2, 1, 12, 28, 22, 824, DateTimeKind.Utc).AddTicks(9949));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000011"),
                column: "UpdatedAt",
                value: new DateTime(2026, 2, 1, 12, 28, 22, 824, DateTimeKind.Utc).AddTicks(9960));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000010"),
                column: "UpdatedAt",
                value: new DateTime(2026, 2, 1, 12, 21, 26, 673, DateTimeKind.Utc).AddTicks(3231));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000011"),
                column: "UpdatedAt",
                value: new DateTime(2026, 2, 1, 12, 21, 26, 673, DateTimeKind.Utc).AddTicks(3242));
        }
    }
}
