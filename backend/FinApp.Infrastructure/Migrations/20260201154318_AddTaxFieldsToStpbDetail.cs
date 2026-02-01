using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FinApp.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddTaxFieldsToStpbDetail : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "NilaiBersih",
                table: "StpbDetails",
                type: "decimal(65,30)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "PPH21",
                table: "StpbDetails",
                type: "decimal(65,30)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "PPH22",
                table: "StpbDetails",
                type: "decimal(65,30)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "PPH23",
                table: "StpbDetails",
                type: "decimal(65,30)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "PPN",
                table: "StpbDetails",
                type: "decimal(65,30)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000010"),
                column: "UpdatedAt",
                value: new DateTime(2026, 2, 1, 15, 43, 18, 11, DateTimeKind.Utc).AddTicks(192));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000011"),
                column: "UpdatedAt",
                value: new DateTime(2026, 2, 1, 15, 43, 18, 11, DateTimeKind.Utc).AddTicks(198));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NilaiBersih",
                table: "StpbDetails");

            migrationBuilder.DropColumn(
                name: "PPH21",
                table: "StpbDetails");

            migrationBuilder.DropColumn(
                name: "PPH22",
                table: "StpbDetails");

            migrationBuilder.DropColumn(
                name: "PPH23",
                table: "StpbDetails");

            migrationBuilder.DropColumn(
                name: "PPN",
                table: "StpbDetails");

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
    }
}
