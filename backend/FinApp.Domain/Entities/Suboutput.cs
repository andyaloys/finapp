namespace FinApp.Domain.Entities;

public class Suboutput : BaseEntity
{
    public string Kode { get; set; } = string.Empty;
    public string Nama { get; set; } = string.Empty;
    public bool IsActive { get; set; } = true;
    
    public Guid OutputId { get; set; }
    
    // Navigation properties
    public Output Output { get; set; } = null!;
    public ICollection<Komponen> Komponens { get; set; } = new List<Komponen>();
}
