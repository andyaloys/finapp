namespace FinApp.Domain.Entities;

public class Kegiatan : BaseEntity
{
    public string Kode { get; set; } = string.Empty;
    public string Nama { get; set; } = string.Empty;
    public bool IsActive { get; set; } = true;
    
    public Guid ProgramId { get; set; }
    
    // Navigation properties
    public Program Program { get; set; } = null!;
    public ICollection<Output> Outputs { get; set; } = new List<Output>();
}
