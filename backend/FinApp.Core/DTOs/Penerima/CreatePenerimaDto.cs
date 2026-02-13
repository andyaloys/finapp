namespace FinApp.Core.DTOs.Penerima;

public class CreatePenerimaDto
{
    public string Nama { get; set; } = string.Empty;
    public string? Npwp { get; set; }
    public string? Alamat { get; set; }
}
