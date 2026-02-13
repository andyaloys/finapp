namespace FinApp.Core.DTOs.Penerima;

public class PenerimaDto
{
    public int Id { get; set; }
    public string Nama { get; set; } = string.Empty;
    public string? Npwp { get; set; }
    public string? Alamat { get; set; }
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}
