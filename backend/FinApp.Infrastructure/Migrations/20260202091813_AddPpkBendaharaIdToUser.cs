using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FinApp.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddPpkBendaharaIdToUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "PpkBendaharaId",
                table: "Users",
                type: "char(36)",
                nullable: true,
                collation: "ascii_general_ci");

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

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000001"),
                column: "PpkBendaharaId",
                value: null);

            migrationBuilder.CreateIndex(
                name: "IX_Users_PpkBendaharaId",
                table: "Users",
                column: "PpkBendaharaId");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_PpkBendahara_PpkBendaharaId",
                table: "Users",
                column: "PpkBendaharaId",
                principalTable: "PpkBendahara",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Users_PpkBendahara_PpkBendaharaId",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Users_PpkBendaharaId",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "PpkBendaharaId",
                table: "Users");

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
    }
}
