namespace FinApp.Domain.Entities;

public class Akun : BaseEntity
{
    public string Kode { get; set; } = string.Empty;
    public string Nama { get; set; } = string.Empty;
    public bool IsActive { get; set; } = true;
    
    public Guid SubkomponenId { get; set; }
    
    // Navigation properties
    public Subkomponen Subkomponen { get; set; } = null!;
    public ICollection<Item> Items { get; set; } = new List<Item>();
}
