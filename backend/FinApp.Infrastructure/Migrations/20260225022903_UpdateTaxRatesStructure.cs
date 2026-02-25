using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FinApp.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateTaxRatesStructure : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Rename TaxCode to TaxType
            migrationBuilder.RenameColumn(
                name: "TaxCode",
                table: "TaxRates",
                newName: "TaxType");

            // Rename TaxName to Category  
            migrationBuilder.RenameColumn(
                name: "TaxName",
                table: "TaxRates",
                newName: "Category");

            // Modify Category column to have max length 100
            migrationBuilder.AlterColumn<string>(
                name: "Category",
                table: "TaxRates",
                type: "varchar(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "longtext")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            // Add new columns
            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "TaxRates",
                type: "varchar(500)",
                maxLength: 500,
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "ReferenceCode",
                table: "TaxRates",
                type: "varchar(50)",
                maxLength: 50,
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<bool>(
                name: "IsDefault",
                table: "TaxRates",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "DisplayOrder",
                table: "TaxRates",
                type: "int",
                nullable: false,
                defaultValue: 1);

            migrationBuilder.UpdateData(
                table: "Menus",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000021"),
                column: "CreatedAt",
                value: new DateTime(2026, 2, 25, 2, 29, 2, 608, DateTimeKind.Utc).AddTicks(5742));

            migrationBuilder.UpdateData(
                table: "Menus",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000022"),
                column: "CreatedAt",
                value: new DateTime(2026, 2, 25, 2, 29, 2, 608, DateTimeKind.Utc).AddTicks(5745));

            migrationBuilder.UpdateData(
                table: "Menus",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000023"),
                column: "CreatedAt",
                value: new DateTime(2026, 2, 25, 2, 29, 2, 608, DateTimeKind.Utc).AddTicks(5748));

            migrationBuilder.UpdateData(
                table: "Menus",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000024"),
                column: "CreatedAt",
                value: new DateTime(2026, 2, 25, 2, 29, 2, 608, DateTimeKind.Utc).AddTicks(5751));

            migrationBuilder.UpdateData(
                table: "Menus",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000025"),
                column: "CreatedAt",
                value: new DateTime(2026, 2, 25, 2, 29, 2, 608, DateTimeKind.Utc).AddTicks(5793));

            migrationBuilder.UpdateData(
                table: "Menus",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000026"),
                column: "CreatedAt",
                value: new DateTime(2026, 2, 25, 2, 29, 2, 608, DateTimeKind.Utc).AddTicks(5796));

            migrationBuilder.UpdateData(
                table: "Menus",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000027"),
                column: "CreatedAt",
                value: new DateTime(2026, 2, 25, 2, 29, 2, 608, DateTimeKind.Utc).AddTicks(5799));

            migrationBuilder.UpdateData(
                table: "Menus",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000028"),
                column: "CreatedAt",
                value: new DateTime(2026, 2, 25, 2, 29, 2, 608, DateTimeKind.Utc).AddTicks(5802));

            migrationBuilder.UpdateData(
                table: "Menus",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000029"),
                column: "CreatedAt",
                value: new DateTime(2026, 2, 25, 2, 29, 2, 608, DateTimeKind.Utc).AddTicks(5804));

            migrationBuilder.UpdateData(
                table: "Menus",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000030"),
                column: "CreatedAt",
                value: new DateTime(2026, 2, 25, 2, 29, 2, 608, DateTimeKind.Utc).AddTicks(5807));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Drop new columns
            migrationBuilder.DropColumn(
                name: "Description",
                table: "TaxRates");

            migrationBuilder.DropColumn(
                name: "ReferenceCode",
                table: "TaxRates");

            migrationBuilder.DropColumn(
                name: "IsDefault",
                table: "TaxRates");

            migrationBuilder.DropColumn(
                name: "DisplayOrder",
                table: "TaxRates");

            // Rename Category back to TaxName
            migrationBuilder.RenameColumn(
                name: "Category",
                table: "TaxRates",
                newName: "TaxName");

            // Rename TaxType back to TaxCode
            migrationBuilder.RenameColumn(
                name: "TaxType",
                table: "TaxRates",
                newName: "TaxCode");

            // Restore TaxName column type
            migrationBuilder.AlterColumn<string>(
                name: "TaxName",
                table: "TaxRates",
                type: "longtext",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(100)",
                oldMaxLength: 100)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.UpdateData(
                table: "Menus",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000021"),
                column: "CreatedAt",
                value: new DateTime(2026, 2, 25, 2, 24, 47, 784, DateTimeKind.Utc).AddTicks(1413));

            migrationBuilder.UpdateData(
                table: "Menus",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000022"),
                column: "CreatedAt",
                value: new DateTime(2026, 2, 25, 2, 24, 47, 784, DateTimeKind.Utc).AddTicks(1416));

            migrationBuilder.UpdateData(
                table: "Menus",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000023"),
                column: "CreatedAt",
                value: new DateTime(2026, 2, 25, 2, 24, 47, 784, DateTimeKind.Utc).AddTicks(1419));

            migrationBuilder.UpdateData(
                table: "Menus",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000024"),
                column: "CreatedAt",
                value: new DateTime(2026, 2, 25, 2, 24, 47, 784, DateTimeKind.Utc).AddTicks(1422));

            migrationBuilder.UpdateData(
                table: "Menus",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000025"),
                column: "CreatedAt",
                value: new DateTime(2026, 2, 25, 2, 24, 47, 784, DateTimeKind.Utc).AddTicks(1424));

            migrationBuilder.UpdateData(
                table: "Menus",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000026"),
                column: "CreatedAt",
                value: new DateTime(2026, 2, 25, 2, 24, 47, 784, DateTimeKind.Utc).AddTicks(1427));

            migrationBuilder.UpdateData(
                table: "Menus",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000027"),
                column: "CreatedAt",
                value: new DateTime(2026, 2, 25, 2, 24, 47, 784, DateTimeKind.Utc).AddTicks(1429));

            migrationBuilder.UpdateData(
                table: "Menus",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000028"),
                column: "CreatedAt",
                value: new DateTime(2026, 2, 25, 2, 24, 47, 784, DateTimeKind.Utc).AddTicks(1432));

            migrationBuilder.UpdateData(
                table: "Menus",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000029"),
                column: "CreatedAt",
                value: new DateTime(2026, 2, 25, 2, 24, 47, 784, DateTimeKind.Utc).AddTicks(1434));

            migrationBuilder.UpdateData(
                table: "Menus",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000030"),
                column: "CreatedAt",
                value: new DateTime(2026, 2, 25, 2, 24, 47, 784, DateTimeKind.Utc).AddTicks(1437));
        }
    }
}
