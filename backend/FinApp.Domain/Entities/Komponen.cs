namespace FinApp.Domain.Entities;

public class Komponen : BaseEntity
{
    public string Kode { get; set; } = string.Empty;
    public string Nama { get; set; } = string.Empty;
    public bool IsActive { get; set; } = true;
    
    public Guid SuboutputId { get; set; }
    
    // Navigation properties
    public Suboutput Suboutput { get; set; } = null!;
    public ICollection<Subkomponen> Subkomponens { get; set; } = new List<Subkomponen>();
}
