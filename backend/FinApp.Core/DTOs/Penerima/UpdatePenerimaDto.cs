namespace FinApp.Core.DTOs.Penerima;

public class UpdatePenerimaDto
{
    public string Nama { get; set; } = string.Empty;
    public string? Npwp { get; set; }
    public string? Alamat { get; set; }
    public bool IsActive { get; set; } = true;
}
