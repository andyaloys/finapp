namespace FinApp.Core.DTOs.Stpb;

public class CreateStpbDto
{
    public string NomorSTPB { get; set; } = string.Empty;
    public DateTime Tanggal { get; set; }
    public string? Deskripsi { get; set; }
    public decimal NilaiTotal { get; set; }
    public string Status { get; set; } = "Draft";
}
