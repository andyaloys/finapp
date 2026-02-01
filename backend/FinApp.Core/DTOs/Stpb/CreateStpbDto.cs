namespace FinApp.Core.DTOs.Stpb;

public class CreateStpbDto
{
    public string NomorSTPB { get; set; } = string.Empty;
    public DateTime TanggalSTPB { get; set; }
    public int Tahun { get; set; }
    public Guid PpkBendaharaId { get; set; }
    public string? Keterangan { get; set; }
    
    // Details akan di-add setelah header dibuat
    public List<CreateStpbDetailDto> Details { get; set; } = new();
}
