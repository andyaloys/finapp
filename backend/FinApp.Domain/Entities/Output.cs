namespace FinApp.Domain.Entities;

public class Output : BaseEntity
{
    public string Kode { get; set; } = string.Empty;
    public string Nama { get; set; } = string.Empty;
    public bool IsActive { get; set; } = true;
    
    public Guid KegiatanId { get; set; }
    
    // Navigation properties
    public Kegiatan Kegiatan { get; set; } = null!;
    public ICollection<Suboutput> Suboutputs { get; set; } = new List<Suboutput>();
}
