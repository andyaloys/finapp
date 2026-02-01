using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FinApp.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RefactorStpbToHeaderDetail : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_STPB_Items_ItemId",
                table: "STPB");

            migrationBuilder.DropIndex(
                name: "IX_STPB_IsLocked",
                table: "STPB");

            migrationBuilder.DropIndex(
                name: "IX_STPB_ItemId",
                table: "STPB");

            migrationBuilder.DropColumn(
                name: "IsLocked",
                table: "STPB");

            migrationBuilder.DropColumn(
                name: "ItemId",
                table: "STPB");

            migrationBuilder.DropColumn(
                name: "KodeAkun",
                table: "STPB");

            migrationBuilder.DropColumn(
                name: "KodeKegiatan",
                table: "STPB");

            migrationBuilder.DropColumn(
                name: "KodeKomponen",
                table: "STPB");

            migrationBuilder.DropColumn(
                name: "KodeOutput",
                table: "STPB");

            migrationBuilder.DropColumn(
                name: "KodeProgram",
                table: "STPB");

            migrationBuilder.DropColumn(
                name: "KodeSubkomponen",
                table: "STPB");

            migrationBuilder.DropColumn(
                name: "KodeSuboutput",
                table: "STPB");

            migrationBuilder.DropColumn(
                name: "NamaItem",
                table: "STPB");

            migrationBuilder.DropColumn(
                name: "NilaiBersih",
                table: "STPB");

            migrationBuilder.DropColumn(
                name: "NoItem",
                table: "STPB");

            migrationBuilder.DropColumn(
                name: "Nominal",
                table: "STPB");

            migrationBuilder.DropColumn(
                name: "PPh21",
                table: "STPB");

            migrationBuilder.DropColumn(
                name: "PPh22",
                table: "STPB");

            migrationBuilder.DropColumn(
                name: "PPh23",
                table: "STPB");

            migrationBuilder.DropColumn(
                name: "Uraian",
                table: "STPB");

            migrationBuilder.RenameColumn(
                name: "Tanggal",
                table: "STPB",
                newName: "TanggalSTPB");

            migrationBuilder.RenameColumn(
                name: "PPn",
                table: "STPB",
                newName: "TotalNilai");

            migrationBuilder.RenameIndex(
                name: "IX_STPB_Tanggal",
                table: "STPB",
                newName: "IX_STPB_TanggalSTPB");

            migrationBuilder.AddColumn<string>(
                name: "Keterangan",
                table: "STPB",
                type: "text",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<Guid>(
                name: "PpkBendaharaId",
                table: "STPB",
                type: "char(36)",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                collation: "ascii_general_ci");

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "STPB",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Tahun",
                table: "STPB",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "PpkBendahara",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    Nama = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    NIP = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Jabatan = table.Column<int>(type: "int", nullable: false),
                    IsActive = table.Column<bool>(type: "tinyint(1)", nullable: false, defaultValue: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.ComputedColumn)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PpkBendahara", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "StpbDetails",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    StpbId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    Tahun = table.Column<int>(type: "int", nullable: false),
                    Revisi = table.Column<int>(type: "int", nullable: false),
                    KodeProgram = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    NamaProgram = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    KodeKegiatan = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    NamaKegiatan = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    KodeOutput = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    NamaOutput = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    KodeSuboutput = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    NamaSuboutput = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    KodeKomponen = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    NamaKomponen = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    KodeSubkomponen = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    NamaSubkomponen = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    KodeAkun = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    NamaAkun = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ItemId = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci"),
                    NoItem = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    NamaItem = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Volume = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Satuan = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    HargaSatuan = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    JumlahHarga = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Keterangan = table.Column<string>(type: "text", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.ComputedColumn)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StpbDetails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StpbDetails_Items_ItemId",
                        column: x => x.ItemId,
                        principalTable: "Items",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_StpbDetails_STPB_StpbId",
                        column: x => x.StpbId,
                        principalTable: "STPB",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

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

            migrationBuilder.CreateIndex(
                name: "IX_STPB_PpkBendaharaId",
                table: "STPB",
                column: "PpkBendaharaId");

            migrationBuilder.CreateIndex(
                name: "IX_STPB_Status",
                table: "STPB",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_STPB_Tahun",
                table: "STPB",
                column: "Tahun");

            migrationBuilder.CreateIndex(
                name: "IX_PpkBendahara_IsActive",
                table: "PpkBendahara",
                column: "IsActive");

            migrationBuilder.CreateIndex(
                name: "IX_PpkBendahara_Nama",
                table: "PpkBendahara",
                column: "Nama");

            migrationBuilder.CreateIndex(
                name: "IX_PpkBendahara_NIP",
                table: "PpkBendahara",
                column: "NIP",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_StpbDetails_ItemId",
                table: "StpbDetails",
                column: "ItemId");

            migrationBuilder.CreateIndex(
                name: "IX_StpbDetails_KodeSuboutput",
                table: "StpbDetails",
                column: "KodeSuboutput");

            migrationBuilder.CreateIndex(
                name: "IX_StpbDetails_StpbId",
                table: "StpbDetails",
                column: "StpbId");

            migrationBuilder.AddForeignKey(
                name: "FK_STPB_PpkBendahara_PpkBendaharaId",
                table: "STPB",
                column: "PpkBendaharaId",
                principalTable: "PpkBendahara",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_STPB_PpkBendahara_PpkBendaharaId",
                table: "STPB");

            migrationBuilder.DropTable(
                name: "PpkBendahara");

            migrationBuilder.DropTable(
                name: "StpbDetails");

            migrationBuilder.DropIndex(
                name: "IX_STPB_PpkBendaharaId",
                table: "STPB");

            migrationBuilder.DropIndex(
                name: "IX_STPB_Status",
                table: "STPB");

            migrationBuilder.DropIndex(
                name: "IX_STPB_Tahun",
                table: "STPB");

            migrationBuilder.DropColumn(
                name: "Keterangan",
                table: "STPB");

            migrationBuilder.DropColumn(
                name: "PpkBendaharaId",
                table: "STPB");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "STPB");

            migrationBuilder.DropColumn(
                name: "Tahun",
                table: "STPB");

            migrationBuilder.RenameColumn(
                name: "TotalNilai",
                table: "STPB",
                newName: "PPn");

            migrationBuilder.RenameColumn(
                name: "TanggalSTPB",
                table: "STPB",
                newName: "Tanggal");

            migrationBuilder.RenameIndex(
                name: "IX_STPB_TanggalSTPB",
                table: "STPB",
                newName: "IX_STPB_Tanggal");

            migrationBuilder.AddColumn<bool>(
                name: "IsLocked",
                table: "STPB",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<Guid>(
                name: "ItemId",
                table: "STPB",
                type: "char(36)",
                nullable: true,
                collation: "ascii_general_ci");

            migrationBuilder.AddColumn<string>(
                name: "KodeAkun",
                table: "STPB",
                type: "varchar(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "KodeKegiatan",
                table: "STPB",
                type: "varchar(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "KodeKomponen",
                table: "STPB",
                type: "varchar(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "KodeOutput",
                table: "STPB",
                type: "varchar(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "KodeProgram",
                table: "STPB",
                type: "varchar(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "KodeSubkomponen",
                table: "STPB",
                type: "varchar(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "KodeSuboutput",
                table: "STPB",
                type: "varchar(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "NamaItem",
                table: "STPB",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<decimal>(
                name: "NilaiBersih",
                table: "STPB",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0.00m);

            migrationBuilder.AddColumn<string>(
                name: "NoItem",
                table: "STPB",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<decimal>(
                name: "Nominal",
                table: "STPB",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0.00m);

            migrationBuilder.AddColumn<decimal>(
                name: "PPh21",
                table: "STPB",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0.00m);

            migrationBuilder.AddColumn<decimal>(
                name: "PPh22",
                table: "STPB",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0.00m);

            migrationBuilder.AddColumn<decimal>(
                name: "PPh23",
                table: "STPB",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0.00m);

            migrationBuilder.AddColumn<string>(
                name: "Uraian",
                table: "STPB",
                type: "text",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000010"),
                column: "UpdatedAt",
                value: new DateTime(2026, 1, 27, 2, 35, 36, 645, DateTimeKind.Utc).AddTicks(8195));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000011"),
                column: "UpdatedAt",
                value: new DateTime(2026, 1, 27, 2, 35, 36, 645, DateTimeKind.Utc).AddTicks(8206));

            migrationBuilder.CreateIndex(
                name: "IX_STPB_IsLocked",
                table: "STPB",
                column: "IsLocked");

            migrationBuilder.CreateIndex(
                name: "IX_STPB_ItemId",
                table: "STPB",
                column: "ItemId");

            migrationBuilder.AddForeignKey(
                name: "FK_STPB_Items_ItemId",
                table: "STPB",
                column: "ItemId",
                principalTable: "Items",
                principalColumn: "Id");
        }
    }
}
