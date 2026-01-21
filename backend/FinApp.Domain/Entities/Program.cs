namespace FinApp.Domain.Entities;

public class Program : BaseEntity
{
    public string Kode { get; set; } = string.Empty;
    public string Nama { get; set; } = string.Empty;
    public bool IsActive { get; set; } = true;
    
    // Navigation properties
    public ICollection<Kegiatan> Kegiatans { get; set; } = new List<Kegiatan>();
}
