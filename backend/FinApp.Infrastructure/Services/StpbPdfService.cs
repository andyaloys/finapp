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
        
        // Load PpkBendahara untuk mendapatkan nama, jabatan, dan NIP
        var ppkBendahara = await _unitOfWork.PpkBendaharas.GetByIdAsync(stpb.PpkBendaharaId);
        if (ppkBendahara == null)
            throw new FileNotFoundException($"PPK/Bendahara tidak ditemukan");

        var document = Document.Create(container =>
        {
            container.Page(page =>
            {
                page.Size(PageSizes.A4);
                page.Margin(40);
                page.PageColor(Colors.White);
                page.DefaultTextStyle(x => x.FontSize(10).FontFamily("Times New Roman"));

                page.Header().Element(content => ComposeHeader(content, stpb, ppkBendahara));
                page.Content().Element(content => ComposeContent(content, stpb, details));
            });
        });

        return document.GeneratePdf();
    }

    private void ComposeHeader(IContainer container, Domain.Entities.Stpb stpb, Domain.Entities.PpkBendahara ppkBendahara)
    {
        // Tentukan jenis surat (LS atau UP)
        var jenisSurat = ppkBendahara.Jabatan == Domain.Entities.JabatanType.PPK ? "LS" : "UP";
        
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
            
            // Nomor SPTB dan jenis surat (LS atau UP) digabung dengan 3 spasi
            column.Item().AlignCenter().PaddingTop(5).Text(text =>
            {
                text.Span("Nomor : ").FontSize(10);
                text.Span(stpb.NomorSTPB).FontSize(10);
                text.Span("   ").FontSize(10); // 3 spasi
                text.Span($"({jenisSurat})").FontSize(10).Bold();
            });
        });
    }

    private void ComposeContent(IContainer container, Domain.Entities.Stpb stpb, IEnumerable<Domain.Entities.StpbDetail> details)
    {
        // Get PPK/Bendahara info for signature
        var ppkBendahara = stpb.PpkBendahara;
        var jabatanDisplay = ppkBendahara.Jabatan == Domain.Entities.JabatanType.PPK 
            ? "Pejabat Pembuat Komitmen" 
            : "Bendahara Pengeluaran";
        
        container.PaddingTop(20).Column(column =>
        {
            // Details section - removed points 1-5
            // Statement paragraph
            column.Item().Text("Yang bertanda tangan di bawah ini Pejabat Pembuat Komitmen Direktorat Jenderal Pengelolaan Pembiayaan dan Risiko menyatakan bahwa saya bertanggung jawab penuh atas segala pengeluaran yang dibayarkan kepada yang berhak menerima dengan perincian sebagai berikut:")
                .FontSize(10).LineHeight(1.5f);

            // Detail table
            column.Item().PaddingTop(15).Table(table =>
            {
                // Define columns with COA
                table.ColumnsDefinition(columns =>
                {
                    columns.ConstantColumn(30);    // No
                    columns.RelativeColumn(2);      // COA
                    columns.RelativeColumn(3);      // Uraian (dikurangi untuk memberi ruang ke kolom angka)
                    columns.RelativeColumn(2);      // Jumlah (dapat menampung 9.000.000.000)
                    columns.RelativeColumn(1.5f);   // PPN (dapat menampung 900.000.000)
                    columns.RelativeColumn(1.5f);   // PPh (dapat menampung 900.000.000)
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
                    var totalPph = detail.PPH21 + detail.PPH22 + detail.PPH23;
                    
                    // Build COA string
                    var coa = $"{detail.KodeProgram}.{detail.KodeKegiatan}.{detail.KodeOutput}.{detail.KodeSuboutput}.{detail.KodeKomponen}.{detail.KodeSubkomponen}.{detail.KodeAkun}.{detail.NoItem}";

                    table.Cell().Border(1).Padding(5).AlignCenter().Text((i + 1).ToString());
                    table.Cell().Border(1).Padding(5).Text(coa).FontSize(8);
                    table.Cell().Border(1).Padding(5).Text(detail.Keterangan ?? "-");
                    table.Cell().Border(1).Padding(5).AlignRight().Text($"{detail.JumlahHarga:N0}");
                    table.Cell().Border(1).Padding(5).AlignRight().Text($"{detail.PPN:N0}");
                    table.Cell().Border(1).Padding(5).AlignRight().Text($"{totalPph:N0}");
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

            column.Item().PaddingTop(5).Text("Demikian surat pernyataan ini dibuat dengan sebenarnya.")
                .FontSize(10);
            
            // Signature section - moved from footer to content
            column.Item().AlignRight().PaddingTop(10).Column(signatureColumn =>
            {
                signatureColumn.Item().Text($"Jakarta, {stpb.TanggalSTPB:dd MMMM yyyy}");
                signatureColumn.Item().Text(jabatanDisplay);
                signatureColumn.Item().PaddingTop(60).Text("Ditandatangani secara elektronik").FontSize(8).Italic();
                signatureColumn.Item().Text(ppkBendahara.Nama);
                signatureColumn.Item().Text($"NIP {ppkBendahara.NIP}");
            });
        });
    }

    private void ComposeFooter(IContainer container, Domain.Entities.Stpb stpb, Domain.Entities.PpkBendahara ppkBendahara)
    {
        // Tentukan jabatan display
        var jabatanDisplay = ppkBendahara.Jabatan == Domain.Entities.JabatanType.PPK 
            ? "Pejabat Pembuat Komitmen" 
            : "Bendahara Pengeluaran";
        
        container.AlignRight().Column(column =>
        {
            column.Item().Text($"Jakarta, {stpb.TanggalSTPB:dd MMMM yyyy}");
            column.Item().Text(jabatanDisplay);
            column.Item().PaddingTop(60).Text("Ditandatangani secara elektronik").FontSize(8).Italic();
            column.Item().Text(ppkBendahara.Nama);
            column.Item().Text($"NIP {ppkBendahara.NIP}");
        });
    }
}