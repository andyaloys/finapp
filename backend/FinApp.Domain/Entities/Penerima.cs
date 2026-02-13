namespace FinApp.Domain.Entities;

public class Penerima : BaseEntity
{
    public new int Id { get; set; }
    public string Nama { get; set; } = string.Empty;
    public string? Npwp { get; set; }
    public string? Alamat { get; set; }
    public bool IsActive { get; set; } = true;
    
    // Navigation property
    public ICollection<StpbDetail> Stpbs { get; set; } = new List<StpbDetail>();
}
