using FinApp.Core.DTOs.PpkBendahara;

namespace FinApp.Core.DTOs.Stpb;

public class StpbDto
{
    public Guid Id { get; set; }
    public string NomorSTPB { get; set; } = string.Empty;
    public DateTime TanggalSTPB { get; set; }
    public int Tahun { get; set; }
    public int Status { get; set; }
    public string StatusName { get; set; } = string.Empty;
    
    public Guid PpkBendaharaId { get; set; }
    public PpkBendaharaDto? PpkBendahara { get; set; }
    
    public decimal TotalNilai { get; set; }
    public string? Keterangan { get; set; }
    
    public Guid CreatedBy { get; set; }
    public string CreatorName { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    
    public List<StpbDetailDto> Details { get; set; } = new();
}
