using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace FinApp.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class FixRoleMenuPermissionRoleIdType : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Menus",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    Key = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Label = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Icon = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ParentKey = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Order = table.Column<int>(type: "int", nullable: false),
                    IsActive = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Menus", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "RoleMenuPermissions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    RoleId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    MenuKey = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    IsVisible = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    MenuId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RoleMenuPermissions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RoleMenuPermissions_Menus_MenuId",
                        column: x => x.MenuId,
                        principalTable: "Menus",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RoleMenuPermissions_Roles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Roles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.InsertData(
                table: "Menus",
                columns: new[] { "Id", "CreatedAt", "Icon", "IsActive", "Key", "Label", "Order", "ParentKey", "UpdatedAt" },
                values: new object[,]
                {
                    { new Guid("096a9193-fe97-4a84-8ba7-3bd2e1877c88"), new DateTime(2026, 2, 5, 1, 49, 7, 475, DateTimeKind.Utc).AddTicks(8442), "file-done", true, "anggaran", "Anggaran", 2, null, new DateTime(2026, 2, 5, 1, 49, 7, 475, DateTimeKind.Utc).AddTicks(7435) },
                    { new Guid("0dc1cf6f-7e27-402b-813c-3b4de6bbe54e"), new DateTime(2026, 2, 5, 1, 49, 7, 475, DateTimeKind.Utc).AddTicks(8598), "team", true, "master-ppkbendahara", "PPK/Bendahara", 1, "master-data", new DateTime(2026, 2, 5, 1, 49, 7, 475, DateTimeKind.Utc).AddTicks(8595) },
                    { new Guid("154dc217-9dd0-45f9-a8d2-7e3a95a8e9b7"), new DateTime(2026, 2, 5, 1, 49, 7, 475, DateTimeKind.Utc).AddTicks(8593), "database", true, "master-data", "Master Data", 4, null, new DateTime(2026, 2, 5, 1, 49, 7, 475, DateTimeKind.Utc).AddTicks(8589) },
                    { new Guid("242139b9-863a-4519-b42c-eed580c08915"), new DateTime(2026, 2, 5, 1, 49, 7, 475, DateTimeKind.Utc).AddTicks(8587), "bar-chart", true, "monitoring", "Monitoring", 3, null, new DateTime(2026, 2, 5, 1, 49, 7, 475, DateTimeKind.Utc).AddTicks(8581) },
                    { new Guid("2d367bdd-6435-4b82-ba30-4dc4ddb3b82b"), new DateTime(2026, 2, 5, 1, 49, 7, 475, DateTimeKind.Utc).AddTicks(7330), "dollar", true, "transaksi", "Transaksi", 1, null, new DateTime(2026, 2, 5, 1, 49, 7, 475, DateTimeKind.Utc).AddTicks(7260) },
                    { new Guid("3031a6ea-a38c-4c87-9fca-42836b5d22c9"), new DateTime(2026, 2, 5, 1, 49, 7, 475, DateTimeKind.Utc).AddTicks(7340), "file-text", true, "transaksi-stpb", "SPTB", 1, "transaksi", new DateTime(2026, 2, 5, 1, 49, 7, 475, DateTimeKind.Utc).AddTicks(7336) },
                    { new Guid("329c02f3-2a7c-4544-a199-d9e70a47eba2"), new DateTime(2026, 2, 5, 1, 49, 7, 475, DateTimeKind.Utc).AddTicks(8632), "setting", true, "administration", "Administration", 5, null, new DateTime(2026, 2, 5, 1, 49, 7, 475, DateTimeKind.Utc).AddTicks(8600) },
                    { new Guid("3368cc0c-af7f-41d3-b6d6-cea76f72bc39"), new DateTime(2026, 2, 5, 1, 49, 7, 475, DateTimeKind.Utc).AddTicks(8643), "safety", true, "admin-roles", "Role Management", 2, "administration", new DateTime(2026, 2, 5, 1, 49, 7, 475, DateTimeKind.Utc).AddTicks(8640) },
                    { new Guid("b63b1d66-1e5b-4c66-b962-abf9dc47f876"), new DateTime(2026, 2, 5, 1, 49, 7, 475, DateTimeKind.Utc).AddTicks(8638), "user", true, "admin-users", "User Management", 1, "administration", new DateTime(2026, 2, 5, 1, 49, 7, 475, DateTimeKind.Utc).AddTicks(8634) },
                    { new Guid("b9406d3b-2ac6-486d-94f0-6d69cf3a5cf2"), new DateTime(2026, 2, 5, 1, 49, 7, 475, DateTimeKind.Utc).AddTicks(8578), "unordered-list", true, "anggaran-list", "Daftar Anggaran", 1, "anggaran", new DateTime(2026, 2, 5, 1, 49, 7, 475, DateTimeKind.Utc).AddTicks(8468) }
                });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000010"),
                column: "UpdatedAt",
                value: new DateTime(2026, 2, 5, 1, 49, 7, 475, DateTimeKind.Utc).AddTicks(3753));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000011"),
                column: "UpdatedAt",
                value: new DateTime(2026, 2, 5, 1, 49, 7, 475, DateTimeKind.Utc).AddTicks(3765));

            migrationBuilder.CreateIndex(
                name: "IX_RoleMenuPermissions_MenuId",
                table: "RoleMenuPermissions",
                column: "MenuId");

            migrationBuilder.CreateIndex(
                name: "IX_RoleMenuPermissions_RoleId",
                table: "RoleMenuPermissions",
                column: "RoleId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RoleMenuPermissions");

            migrationBuilder.DropTable(
                name: "Menus");

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000010"),
                column: "UpdatedAt",
                value: new DateTime(2026, 2, 5, 1, 2, 26, 927, DateTimeKind.Utc).AddTicks(6877));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000011"),
                column: "UpdatedAt",
                value: new DateTime(2026, 2, 5, 1, 2, 26, 927, DateTimeKind.Utc).AddTicks(6885));
        }
    }
}
