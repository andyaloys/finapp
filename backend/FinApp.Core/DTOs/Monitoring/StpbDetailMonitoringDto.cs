namespace FinApp.Core.DTOs.Monitoring;

public class StpbDetailMonitoringDto
{
    public string NoStpb { get; set; } = string.Empty;
    public DateTime TanggalStpb { get; set; }
    public string Keterangan { get; set; } = string.Empty;
    public string? Penerima { get; set; }
    public decimal NilaiKotor { get; set; }
    public decimal Pajak { get; set; }
    public decimal NilaiBersih { get; set; }
    public string Status { get; set; } = string.Empty;
    public Guid StpbId { get; set; }
}
