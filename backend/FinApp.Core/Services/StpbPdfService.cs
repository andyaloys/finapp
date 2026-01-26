using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using FinApp.Core.DTOs.Stpb;

namespace FinApp.Core.Services;

public class StpbPdfService
{
    public byte[] GenerateStpbPdf(StpbDto stpb)
    {
        QuestPDF.Settings.License = LicenseType.Community;
        
        var document = Document.Create(container =>
        {
            container.Page(page =>
            {
                page.Size(PageSizes.A4);
                page.Margin(40);
                
                page.Content().Column(column =>
                {
                    // Header
                    column.Item().AlignCenter().Text("SURAT PERNYATAAN TANGGUNG JAWAB BELANJA");
                    
                    column.Item().PaddingTop(20);
                    
                    // Paragraf pembuka
                    column.Item().Text(text =>
                    {
                        text.Span("Yang bertandatangan di bawah ini atas nama Kuasa Pengguna Anggaran Satuan Kerja ");
                        text.Span("DIREKTORAT JENDERAL PENGELOLAAN PEMBIAYAAN DAN RISIKO");
                        text.Span(" menyatakan bahwa saya bertanggung jawab secara formal dan material atas segala pengeluaran yang telah dibayar lunas oleh Bendahara Pengeluaran kepada yang berhak menerima, dan kebenaran perhitungan dan setoran pajak yang telah dipungut atas pembayaran tersebut dengan perincian sebagai berikut :");
                    });
                    
                    column.Item().PaddingTop(15);
                    
                    // Tabel Data STPB
                    column.Item().Table(table =>
                    {
                        table.ColumnsDefinition(columns =>
                        {
                            columns.ConstantColumn(100);
                            columns.ConstantColumn(150);
                            columns.ConstantColumn(60);
                            columns.RelativeColumn();
                            columns.ConstantColumn(100);
                        });
                        
                        // Header
                        table.Header(header =>
                        {
                            header.Cell().Border(1).Padding(5).Text("Tanggal");
                            header.Cell().Border(1).Padding(5).Text("Nomer Dokumen");
                            header.Cell().Border(1).Padding(5).Text("Akun");
                            header.Cell().Border(1).Padding(5).Text("Uraian");
                            header.Cell().Border(1).Padding(5).Text("Nominal");
                        });
                        
                        // Data
                        table.Cell().Border(1).Padding(5).Text(stpb.Tanggal.ToString("dd/MM/yyyy"));
                        table.Cell().Border(1).Padding(5).Text(stpb.NomorSTPB);
                        table.Cell().Border(1).Padding(5).Text("521211"); // Placeholder
                        table.Cell().Border(1).Padding(5).Text(stpb.Uraian);
                        table.Cell().Border(1).Padding(5).AlignRight().Text(stpb.NilaiBersih.ToString("N0"));
                    });
                    
                    column.Item().PaddingTop(15);
                    
                    // Paragraf penutup
                    column.Item().Text("Bukti-bukti pengeluaran anggaran dan asli setoran pajak (SSP/BPN) tersebut di atas disimpan oleh Pengguna Anggaran/ Kuasa Pengguna Anggaran untuk kelengkapan administrasi dan pemeriksaan aparat pengawasan fungsional. Demikian surat pernyataan ini dibuat dengan sebenarnya.");
                    
                    column.Item().PaddingTop(30);
                    
                    // Tanda tangan
                    column.Item().Row(row =>
                    {
                        row.RelativeItem().Column(leftCol =>
                        {
                            leftCol.Item().Text("Pejabat Pembuat Komitmen");
                            leftCol.Item().PaddingTop(60);
                            leftCol.Item().Text("Setyo Maulana");
                            leftCol.Item().Text("NIP.197412101995111001");
                        });
                        
                        row.RelativeItem().Column(rightCol =>
                        {
                            rightCol.Item().AlignRight().Text("Bendahara Pengeluaran");
                            rightCol.Item().PaddingTop(60);
                            rightCol.Item().AlignRight().Text("Enriko Sahat Tua Siitonga");
                            rightCol.Item().AlignRight().Text("NIP.199511052016121001");
                        });
                    });
                });
            });
        });

        return document.GeneratePdf();
    }
}
