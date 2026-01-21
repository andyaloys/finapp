using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FinApp.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialWithReferensi : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_STPB_Status",
                table: "STPB");

            migrationBuilder.DropColumn(
                name: "Deskripsi",
                table: "STPB");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "STPB");

            migrationBuilder.RenameColumn(
                name: "NilaiTotal",
                table: "STPB",
                newName: "PPn");

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

            migrationBuilder.AddColumn<decimal>(
                name: "NilaiBersih",
                table: "STPB",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0.00m);

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

            migrationBuilder.CreateTable(
                name: "Programs",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    Kode = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Nama = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    IsActive = table.Column<bool>(type: "tinyint(1)", nullable: false, defaultValue: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Programs", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Kegiatans",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    Kode = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Nama = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    IsActive = table.Column<bool>(type: "tinyint(1)", nullable: false, defaultValue: true),
                    ProgramId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Kegiatans", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Kegiatans_Programs_ProgramId",
                        column: x => x.ProgramId,
                        principalTable: "Programs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Outputs",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    Kode = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Nama = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    IsActive = table.Column<bool>(type: "tinyint(1)", nullable: false, defaultValue: true),
                    KegiatanId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Outputs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Outputs_Kegiatans_KegiatanId",
                        column: x => x.KegiatanId,
                        principalTable: "Kegiatans",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Suboutputs",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    Kode = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Nama = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    IsActive = table.Column<bool>(type: "tinyint(1)", nullable: false, defaultValue: true),
                    OutputId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Suboutputs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Suboutputs_Outputs_OutputId",
                        column: x => x.OutputId,
                        principalTable: "Outputs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Komponens",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    Kode = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Nama = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    IsActive = table.Column<bool>(type: "tinyint(1)", nullable: false, defaultValue: true),
                    SuboutputId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Komponens", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Komponens_Suboutputs_SuboutputId",
                        column: x => x.SuboutputId,
                        principalTable: "Suboutputs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Subkomponens",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    Kode = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Nama = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    IsActive = table.Column<bool>(type: "tinyint(1)", nullable: false, defaultValue: true),
                    KomponenId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Subkomponens", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Subkomponens_Komponens_KomponenId",
                        column: x => x.KomponenId,
                        principalTable: "Komponens",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Akuns",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    Kode = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Nama = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    IsActive = table.Column<bool>(type: "tinyint(1)", nullable: false, defaultValue: true),
                    SubkomponenId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Akuns", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Akuns_Subkomponens_SubkomponenId",
                        column: x => x.SubkomponenId,
                        principalTable: "Subkomponens",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Items",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    Nama = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Satuan = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    HargaSatuan = table.Column<decimal>(type: "decimal(18,2)", nullable: false, defaultValue: 0.00m),
                    IsActive = table.Column<bool>(type: "tinyint(1)", nullable: false, defaultValue: true),
                    AkunId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Items", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Items_Akuns_AkunId",
                        column: x => x.AkunId,
                        principalTable: "Akuns",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_STPB_IsLocked",
                table: "STPB",
                column: "IsLocked");

            migrationBuilder.CreateIndex(
                name: "IX_STPB_ItemId",
                table: "STPB",
                column: "ItemId");

            migrationBuilder.CreateIndex(
                name: "IX_Akuns_IsActive",
                table: "Akuns",
                column: "IsActive");

            migrationBuilder.CreateIndex(
                name: "IX_Akuns_Kode",
                table: "Akuns",
                column: "Kode",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Akuns_Nama",
                table: "Akuns",
                column: "Nama");

            migrationBuilder.CreateIndex(
                name: "IX_Akuns_SubkomponenId",
                table: "Akuns",
                column: "SubkomponenId");

            migrationBuilder.CreateIndex(
                name: "IX_Items_AkunId",
                table: "Items",
                column: "AkunId");

            migrationBuilder.CreateIndex(
                name: "IX_Items_IsActive",
                table: "Items",
                column: "IsActive");

            migrationBuilder.CreateIndex(
                name: "IX_Items_Nama",
                table: "Items",
                column: "Nama");

            migrationBuilder.CreateIndex(
                name: "IX_Kegiatans_IsActive",
                table: "Kegiatans",
                column: "IsActive");

            migrationBuilder.CreateIndex(
                name: "IX_Kegiatans_Kode",
                table: "Kegiatans",
                column: "Kode",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Kegiatans_Nama",
                table: "Kegiatans",
                column: "Nama");

            migrationBuilder.CreateIndex(
                name: "IX_Kegiatans_ProgramId",
                table: "Kegiatans",
                column: "ProgramId");

            migrationBuilder.CreateIndex(
                name: "IX_Komponens_IsActive",
                table: "Komponens",
                column: "IsActive");

            migrationBuilder.CreateIndex(
                name: "IX_Komponens_Kode",
                table: "Komponens",
                column: "Kode",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Komponens_Nama",
                table: "Komponens",
                column: "Nama");

            migrationBuilder.CreateIndex(
                name: "IX_Komponens_SuboutputId",
                table: "Komponens",
                column: "SuboutputId");

            migrationBuilder.CreateIndex(
                name: "IX_Outputs_IsActive",
                table: "Outputs",
                column: "IsActive");

            migrationBuilder.CreateIndex(
                name: "IX_Outputs_KegiatanId",
                table: "Outputs",
                column: "KegiatanId");

            migrationBuilder.CreateIndex(
                name: "IX_Outputs_Kode",
                table: "Outputs",
                column: "Kode",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Outputs_Nama",
                table: "Outputs",
                column: "Nama");

            migrationBuilder.CreateIndex(
                name: "IX_Programs_IsActive",
                table: "Programs",
                column: "IsActive");

            migrationBuilder.CreateIndex(
                name: "IX_Programs_Kode",
                table: "Programs",
                column: "Kode",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Programs_Nama",
                table: "Programs",
                column: "Nama");

            migrationBuilder.CreateIndex(
                name: "IX_Subkomponens_IsActive",
                table: "Subkomponens",
                column: "IsActive");

            migrationBuilder.CreateIndex(
                name: "IX_Subkomponens_Kode",
                table: "Subkomponens",
                column: "Kode",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Subkomponens_KomponenId",
                table: "Subkomponens",
                column: "KomponenId");

            migrationBuilder.CreateIndex(
                name: "IX_Subkomponens_Nama",
                table: "Subkomponens",
                column: "Nama");

            migrationBuilder.CreateIndex(
                name: "IX_Suboutputs_IsActive",
                table: "Suboutputs",
                column: "IsActive");

            migrationBuilder.CreateIndex(
                name: "IX_Suboutputs_Kode",
                table: "Suboutputs",
                column: "Kode",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Suboutputs_Nama",
                table: "Suboutputs",
                column: "Nama");

            migrationBuilder.CreateIndex(
                name: "IX_Suboutputs_OutputId",
                table: "Suboutputs",
                column: "OutputId");

            migrationBuilder.AddForeignKey(
                name: "FK_STPB_Items_ItemId",
                table: "STPB",
                column: "ItemId",
                principalTable: "Items",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_STPB_Items_ItemId",
                table: "STPB");

            migrationBuilder.DropTable(
                name: "Items");

            migrationBuilder.DropTable(
                name: "Akuns");

            migrationBuilder.DropTable(
                name: "Subkomponens");

            migrationBuilder.DropTable(
                name: "Komponens");

            migrationBuilder.DropTable(
                name: "Suboutputs");

            migrationBuilder.DropTable(
                name: "Outputs");

            migrationBuilder.DropTable(
                name: "Kegiatans");

            migrationBuilder.DropTable(
                name: "Programs");

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
                name: "NilaiBersih",
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
                name: "PPn",
                table: "STPB",
                newName: "NilaiTotal");

            migrationBuilder.AddColumn<string>(
                name: "Deskripsi",
                table: "STPB",
                type: "text",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "STPB",
                type: "varchar(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "Draft")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_STPB_Status",
                table: "STPB",
                column: "Status");
        }
    }
}
