namespace FinApp.Core.DTOs.Stpb;

public class StpbDto
{
    public Guid Id { get; set; }
    public string NomorSTPB { get; set; } = string.Empty;
    public DateTime Tanggal { get; set; }
    public string Uraian { get; set; } = string.Empty;
    public decimal Nominal { get; set; }
    public decimal Ppn { get; set; }
    public decimal Pph21 { get; set; }
    public decimal Pph22 { get; set; }
    public decimal Pph23 { get; set; }
    public decimal NilaiBersih { get; set; }
    public string? NoItem { get; set; }
    public string? NamaItem { get; set; }
    public string Status { get; set; } = string.Empty;
    public Guid CreatedBy { get; set; }
    public string CreatorName { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}
