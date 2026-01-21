namespace FinApp.Core.DTOs.Stpb;

public class StpbDto
{
    public Guid Id { get; set; }
    public string NomorSTPB { get; set; } = string.Empty;
    public DateTime Tanggal { get; set; }
    public string? Deskripsi { get; set; }
    public decimal NilaiTotal { get; set; }
    public string Status { get; set; } = string.Empty;
    public Guid CreatedBy { get; set; }
    public string CreatorName { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}
