namespace FinApp.Core.DTOs.Stpb;

public class StpbDetailDto
{
    public Guid Id { get; set; }
    
    // Denormalized Anggaran Structure
    public int Tahun { get; set; }
    public int Revisi { get; set; }
    public string KodeProgram { get; set; } = string.Empty;
    public string NamaProgram { get; set; } = string.Empty;
    public string KodeKegiatan { get; set; } = string.Empty;
    public string NamaKegiatan { get; set; } = string.Empty;
    public string KodeOutput { get; set; } = string.Empty;
    public string NamaOutput { get; set; } = string.Empty;
    public string KodeSuboutput { get; set; } = string.Empty;
    public string NamaSuboutput { get; set; } = string.Empty;
    public string KodeKomponen { get; set; } = string.Empty;
    public string NamaKomponen { get; set; } = string.Empty;
    public string KodeSubkomponen { get; set; } = string.Empty;
    public string NamaSubkomponen { get; set; } = string.Empty;
    public string KodeAkun { get; set; } = string.Empty;
    public string NamaAkun { get; set; } = string.Empty;
    public string? NoItem { get; set; }
    public string? NamaItem { get; set; }
    
    // Transaction Details
    public DateTime TanggalTransaksi { get; set; }
    public decimal Volume { get; set; }
    public string Satuan { get; set; } = string.Empty;
    public decimal HargaSatuan { get; set; }
    public decimal JumlahHarga { get; set; }
    public string? Keterangan { get; set; }
    public string? Penerima { get; set; }
    
    // Tax/Potongan
    public decimal PPN { get; set; }
    public decimal PPH21 { get; set; }
    public decimal PPH22 { get; set; }
    public decimal PPH23 { get; set; }
    public decimal NilaiBersih { get; set; }
}
