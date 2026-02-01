using FinApp.Core.Interfaces;
using FinApp.Domain.Interfaces;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace FinApp.Infrastructure.Services;

public class StpbPdfService : IStpbPdfService
{
    private readonly IUnitOfWork _unitOfWork;

    public StpbPdfService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
        QuestPDF.Settings.License = LicenseType.Community;
    }

    public async Task<byte[]> GenerateStpbPdfAsync(Guid stpbId)
    {
        var stpb = await _unitOfWork.Stpbs.GetByIdAsync(stpbId);
        if (stpb == null)
            throw new FileNotFoundException($"STPB dengan ID {stpbId} tidak ditemukan");

        var details = await _unitOfWork.StpbDetails.GetByStpbIdAsync(stpbId);

        var document = Document.Create(container =>
        {
            container.Page(page =>
            {
                page.Size(PageSizes.A4);
                page.Margin(40);
                page.PageColor(Colors.White);
                page.DefaultTextStyle(x => x.FontSize(10).FontFamily("Times New Roman"));

                page.Header().Element(ComposeHeader);
                page.Content().Element(content => ComposeContent(content, stpb, details));
                page.Footer().Element(footer => ComposeFooter(footer, stpb));
            });
        });

        return document.GeneratePdf();
    }

    private void ComposeHeader(IContainer container)
    {
        container.Column(column =>
        {
            // Header text
            column.Item().AlignCenter().Text(text =>
            {
                text.Span("KEMENTERIAN KEUANGAN REPUBLIK INDONESIA\n").Bold().FontSize(11);
                text.Span("DIREKTORAT JENDERAL PENGELOLAAN PEMBIAYAAN DAN RISIKO\n").Bold().FontSize(11);
                text.Span("SEKRETARIAT DIREKTORAT JENDERAL").Bold().FontSize(11);
            });

            column.Item().PaddingTop(5).AlignCenter().Text(text =>
            {
                text.Span("GEDUNG FRANS SEDA LT. 2 JALAN DR. WAHIDIN RAYA NOMOR 1 JAKARTA 10710\n").FontSize(9);
                text.Span("TELEPON (021) 3500843 atau www.djpu.kemenkeu.go.id").FontSize(9);
            });

            // Divider
            column.Item().PaddingTop(10).PaddingBottom(10).LineHorizontal(1);

            // Document title
            column.Item().AlignCenter().PaddingTop(10).Text("SURAT PERNYATAAN TANGGUNG JAWAB BELANJA")
                .Bold().FontSize(12);
            
            column.Item().AlignCenter().PaddingTop(5).Text(text =>
            {
                text.Span("Nomor : SPTB-").FontSize(10);
                text.Span("___").Underline().FontSize(10);
                text.Span("/FPK/DJPFR/2026").FontSize(10);
            });
        });
    }

    private void ComposeContent(IContainer container, Domain.Entities.Stpb stpb, IEnumerable<Domain.Entities.StpbDetail> details)
    {
        container.PaddingTop(20).Column(column =>
        {
            // Details section
            column.Item().Table(table =>
            {
                table.ColumnsDefinition(columns =>
                {
                    columns.ConstantColumn(20);
                    columns.RelativeColumn(2);
                    columns.ConstantColumn(10);
                    columns.RelativeColumn(3);
                });

                table.Cell().Row(1).Column(1).Text("1.");
                table.Cell().Row(1).Column(2).Text("Kode Satuan Kerja");
                table.Cell().Row(1).Column(3).Text(":");
                table.Cell().Row(1).Column(4).Text("455422");

                table.Cell().Row(2).Column(1).Text("2.");
                table.Cell().Row(2).Column(2).Text("Nama Satuan Kerja");
                table.Cell().Row(2).Column(3).Text(":");
                table.Cell().Row(2).Column(4).Text("Direktorat Jenderal Pengelolaan Pembiayaan dan Risiko");

                table.Cell().Row(3).Column(1).Text("3.");
                table.Cell().Row(3).Column(2).Text("Nomor DIPA");
                table.Cell().Row(3).Column(3).Text(":");
                table.Cell().Row(3).Column(4).Text("SP DIPA-015.07.1.455401/2026");

                table.Cell().Row(4).Column(1).Text("4.");
                table.Cell().Row(4).Column(2).Text("Tanggal DIPA");
                table.Cell().Row(4).Column(3).Text(":");
                table.Cell().Row(4).Column(4).Text("01 Desember 2025");

                table.Cell().Row(5).Column(1).Text("5.");
                table.Cell().Row(5).Column(2).Text("Maksud Pembayaran");
                table.Cell().Row(5).Column(3).Text(":");
                table.Cell().Row(5).Column(4).Text($"Uang Persediaan (UP) / Langsung (LS)");
            });

            // Statement paragraph
            column.Item().PaddingTop(15).Text("Yang bertanda tangan di bawah ini Pejabat Pembuat Komitmen Direktorat Jenderal Pengelolaan Pembiayaan dan Risiko menyatakan bahwa saya bertanggung jawab penuh atas segala pengeluaran yang dibayarkan kepada yang berhak menerima dengan perincian sebagai berikut:")
                .FontSize(10).LineHeight(1.5f);

            // Detail table
            column.Item().PaddingTop(15).Table(table =>
            {
                // Define columns
                table.ColumnsDefinition(columns =>
                {
                    columns.ConstantColumn(30);    // No
                    columns.RelativeColumn(3);      // COA
                    columns.RelativeColumn(4);      // Uraian
                    columns.RelativeColumn(2);      // Jumlah
                    columns.RelativeColumn(1);      // PPN
                    columns.RelativeColumn(1);      // PPh
                });

                // Header
                table.Header(header =>
                {
                    header.Cell().RowSpan(2).Border(1).Padding(5).AlignCenter().AlignMiddle().Text("No");
                    header.Cell().RowSpan(2).Border(1).Padding(5).AlignCenter().AlignMiddle().Text("COA");
                    header.Cell().RowSpan(2).Border(1).Padding(5).AlignCenter().AlignMiddle().Text("Uraian");
                    header.Cell().RowSpan(2).Border(1).Padding(5).AlignCenter().AlignMiddle().Text("Jumlah");
                    header.Cell().ColumnSpan(2).Border(1).Padding(5).AlignCenter().Text("Pajak Pungut / Setor");

                    // Second row of header
                    header.Cell().Border(1).Padding(5).AlignCenter().Text("PPN");
                    header.Cell().Border(1).Padding(5).AlignCenter().Text("PPh");
                });

                // Data rows
                var detailsList = details.ToList();
                for (int i = 0; i < detailsList.Count; i++)
                {
                    var detail = detailsList[i];
                    var coa = $"{detail.KodeProgram}.{detail.KodeKegiatan}.{detail.KodeOutput}.{detail.KodeSuboutput}.{detail.KodeKomponen}.{detail.KodeSubkomponen}.{detail.KodeAkun}.{detail.NoItem}";
                    var totalPph = detail.PPH21 + detail.PPH22 + detail.PPH23;

                    table.Cell().Border(1).Padding(5).AlignCenter().Text((i + 1).ToString());
                    table.Cell().Border(1).Padding(5).Text(text =>
                    {
                        text.Span("Data COA Lengkap sd Level Nama Item\n").FontSize(8);
                        text.Span(coa).FontSize(8);
                    });
                    table.Cell().Border(1).Padding(5).Text(detail.NamaItem ?? "-");
                    table.Cell().Border(1).Padding(5).AlignRight().Text($"{detail.JumlahHarga:N0}");
                    table.Cell().Border(1).Padding(5).AlignRight().Text($"{detail.PPN:N0}");
                    table.Cell().Border(1).Padding(5).AlignRight().Text($"{totalPph:N0}");
                }

                // Empty rows
                for (int i = detailsList.Count; i < 3; i++)
                {
                    table.Cell().Border(1).Padding(5).AlignCenter().Text((i + 1).ToString());
                    table.Cell().Border(1).Padding(5).Text("");
                    table.Cell().Border(1).Padding(5).Text("");
                    table.Cell().Border(1).Padding(5).Text("");
                    table.Cell().Border(1).Padding(5).Text("");
                    table.Cell().Border(1).Padding(5).Text("");
                }

                // Total row
                var totalJumlah = detailsList.Sum(d => d.JumlahHarga);
                table.Cell().ColumnSpan(3).Border(1).Padding(5).AlignCenter().Text("Jumlah").Bold();
                table.Cell().Border(1).Padding(5).AlignRight().Text($"{totalJumlah:N0}").Bold();
                table.Cell().ColumnSpan(2).Border(1).Padding(5).Text("");
            });

            // Closing statement
            column.Item().PaddingTop(15).Text("Bukti – bukti tersebut di atas disimpan sesuai ketentuan yang berlaku pada Satuan Kerja Direktorat Jenderal Pengelolaan Pembiayaan dan Risiko untuk kelengkapan administrasi dan keperluan pemeriksaan aparat pengawas fungsional.")
                .FontSize(10).LineHeight(1.5f);

            column.Item().PaddingTop(10).Text("Demikian surat pernyataan ini dibuat dengan sebenarnya.")
                .FontSize(10);
        });
    }

    private void ComposeFooter(IContainer container, Domain.Entities.Stpb stpb)
    {
        container.AlignRight().PaddingTop(20).Column(column =>
        {
            column.Item().Text($"Jakarta,          {DateTime.Now.Year}");
            column.Item().Text("Pejabat Pembuat Komitmen");
            column.Item().PaddingTop(60).Text("Ditandatangani secara elektronik").FontSize(8).Italic();
            column.Item().Text("Setyo Maulana");
            column.Item().Text("NIP 197412101995111001");
        });
    }
}
