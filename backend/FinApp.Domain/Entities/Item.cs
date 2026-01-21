namespace FinApp.Domain.Entities;

public class Item : BaseEntity
{
    public string Nama { get; set; } = string.Empty;
    public string Satuan { get; set; } = string.Empty;
    public decimal HargaSatuan { get; set; }
    public bool IsActive { get; set; } = true;
    
    public Guid AkunId { get; set; }
    
    // Navigation properties
    public Akun Akun { get; set; } = null!;
}
