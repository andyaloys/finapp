using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using FinApp.Core.DTOs.Stpb;

namespace FinApp.Core.Services;

public class StpbPdfService
{
    public byte[] GenerateStpbPdf(StpbDto stpb)
    {
        // TODO: Refactor for new header-detail structure
        // Will iterate through stpb.Details collection
        // Multiple rows for each StpbDetail
        QuestPDF.Settings.License = LicenseType.Community;
        
        var document = Document.Create(container =>
        {
            container.Page(page =>
            {
                page.Size(PageSizes.A4);
                page.Margin(40);
                
                page.Content().Column(column =>
                {
                    column.Item().AlignCenter().Text("SURAT PERNYATAAN TANGGUNG JAWAB BELANJA");
                    column.Item().PaddingTop(20);
                    column.Item().Text($"Nomor: {stpb.NomorSTPB}");
                    column.Item().Text($"Tanggal: {stpb.TanggalSTPB:dd/MM/yyyy}");
                    column.Item().Text($"Total: Rp {stpb.TotalNilai:N0}");
                    column.Item().PaddingTop(20);
                    column.Item().Text("Detail transaksi:");
                    
                    // Table for details
                    if (stpb.Details?.Any() == true)
                    {
                        foreach (var detail in stpb.Details)
                        {
                            column.Item().Text($"- {detail.NamaItem ?? detail.NamaAkun}: Rp {detail.JumlahHarga:N0}");
                        }
                    }
                });
            });
        });

        return document.GeneratePdf();
    }
}

