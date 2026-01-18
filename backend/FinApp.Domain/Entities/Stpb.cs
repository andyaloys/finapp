namespace FinApp.Domain.Entities;

public class Stpb : BaseEntity
{
    public string NomorSTPB { get; set; } = string.Empty;
    public DateTime Tanggal { get; set; }
    public string? Deskripsi { get; set; }
    public decimal NilaiTotal { get; set; }
    public string Status { get; set; } = "Draft"; // Draft, Submitted, Approved, Rejected
    public int CreatedBy { get; set; }

    // Navigation properties
    public User? Creator { get; set; }
}
