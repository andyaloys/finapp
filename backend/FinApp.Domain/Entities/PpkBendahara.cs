namespace FinApp.Domain.Entities;

public class PpkBendahara : BaseEntity
{
    public string Nama { get; set; } = string.Empty;
    public string NIP { get; set; } = string.Empty;
    public JabatanType Jabatan { get; set; }
    public bool IsActive { get; set; } = true;
    
    // Navigation properties
    public ICollection<Stpb> Stpbs { get; set; } = new List<Stpb>();
}
