namespace FinApp.Core.DTOs.Stpb;

public class CreateStpbDto
{
    public string NomorSTPB { get; set; } = string.Empty;
    public DateTime Tanggal { get; set; }
    public string ProgramId { get; set; } = string.Empty;
    public string KegiatanId { get; set; } = string.Empty;
    public string OutputId { get; set; } = string.Empty;
    public string SuboutputId { get; set; } = string.Empty;
    public string KomponenId { get; set; } = string.Empty;
    public string SubkomponenId { get; set; } = string.Empty;
    public string AkunId { get; set; } = string.Empty;
    public string? ItemId { get; set; }
    public string? NoItem { get; set; }
    public string? NamaItem { get; set; }
    public string Uraian { get; set; } = string.Empty;
    public decimal Nominal { get; set; }
    public decimal Ppn { get; set; }
    public decimal Pph21 { get; set; }
    public decimal Pph22 { get; set; }
    public decimal Pph23 { get; set; }
    public decimal NilaiTotal { get; set; }
    public string Status { get; set; } = "Draft";
}
