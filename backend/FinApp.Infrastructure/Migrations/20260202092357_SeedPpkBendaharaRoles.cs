using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FinApp.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class SeedPpkBendaharaRoles : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000010"),
                column: "UpdatedAt",
                value: new DateTime(2026, 2, 2, 9, 23, 56, 875, DateTimeKind.Utc).AddTicks(47));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000011"),
                column: "UpdatedAt",
                value: new DateTime(2026, 2, 2, 9, 23, 56, 875, DateTimeKind.Utc).AddTicks(58));

            // Insert PPK role
            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "Id", "Name", "Description", "IsAdmin", "CreatedAt", "UpdatedAt" },
                values: new object[] { new Guid("00000000-0000-0000-0000-000000000012"), "PPK", "Pejabat Pembuat Komitmen", false, DateTime.UtcNow, DateTime.UtcNow });

            // Insert Bendahara role
            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "Id", "Name", "Description", "IsAdmin", "CreatedAt", "UpdatedAt" },
                values: new object[] { new Guid("00000000-0000-0000-0000-000000000013"), "Bendahara", "Bendahara Pengeluaran", false, DateTime.UtcNow, DateTime.UtcNow });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000012"));

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000013"));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000010"),
                column: "UpdatedAt",
                value: new DateTime(2026, 2, 2, 9, 18, 12, 737, DateTimeKind.Utc).AddTicks(1528));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000011"),
                column: "UpdatedAt",
                value: new DateTime(2026, 2, 2, 9, 18, 12, 737, DateTimeKind.Utc).AddTicks(1538));
        }
    }
}
