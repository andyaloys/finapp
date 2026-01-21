namespace FinApp.Domain.Entities;

public class Subkomponen : BaseEntity
{
    public string Kode { get; set; } = string.Empty;
    public string Nama { get; set; } = string.Empty;
    public bool IsActive { get; set; } = true;
    
    public Guid KomponenId { get; set; }
    
    // Navigation properties
    public Komponen Komponen { get; set; } = null!;
    public ICollection<Akun> Akuns { get; set; } = new List<Akun>();
}
