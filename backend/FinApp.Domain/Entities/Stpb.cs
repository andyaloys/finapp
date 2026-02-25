namespace FinApp.Domain.Entities;

public class Stpb : BaseEntity
{
    // Header Information
    public string NomorSTPB { get; set; } = string.Empty;
    public DateTime TanggalSTPB { get; set; }
    public int Tahun { get; set; }
    public StpbStatus Status { get; set; } = StpbStatus.Draft;
    
    // Foreign Keys
    public Guid PpkBendaharaId { get; set; }
    public Guid CreatedBy { get; set; }
    
    // Computed/Cached Field
    public decimal TotalNilai { get; set; }
    
    // Additional Information
    public string? Keterangan { get; set; }
    public string? AlasanDikembalikan { get; set; }
    
    // Navigation properties
    public PpkBendahara PpkBendahara { get; set; } = null!;
    public User Creator { get; set; } = null!;
    public ICollection<StpbDetail> StpbDetails { get; set; } = new List<StpbDetail>();
}
