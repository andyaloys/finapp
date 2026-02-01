using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FinApp.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddPenerimaToStpbDetail : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Penerima",
                table: "StpbDetails",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000010"),
                column: "UpdatedAt",
                value: new DateTime(2026, 2, 1, 11, 2, 29, 329, DateTimeKind.Utc).AddTicks(8193));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000011"),
                column: "UpdatedAt",
                value: new DateTime(2026, 2, 1, 11, 2, 29, 329, DateTimeKind.Utc).AddTicks(8205));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Penerima",
                table: "StpbDetails");

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000010"),
                column: "UpdatedAt",
                value: new DateTime(2026, 2, 1, 3, 55, 13, 229, DateTimeKind.Utc).AddTicks(4453));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000011"),
                column: "UpdatedAt",
                value: new DateTime(2026, 2, 1, 3, 55, 13, 229, DateTimeKind.Utc).AddTicks(4464));
        }
    }
}
