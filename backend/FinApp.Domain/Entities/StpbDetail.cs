namespace FinApp.Domain.Entities;

public class StpbDetail : BaseEntity
{
    // Foreign Key
    public Guid StpbId { get; set; }
    
    // Transaction Date
    public DateTime TanggalTransaksi { get; set; }
    
    // Denormalized Anggaran Structure (for historical preservation)
    public int Tahun { get; set; }
    public int Revisi { get; set; }
    
    // Program
    public string KodeProgram { get; set; } = string.Empty;
    public string NamaProgram { get; set; } = string.Empty;
    
    // Kegiatan
    public string KodeKegiatan { get; set; } = string.Empty;
    public string NamaKegiatan { get; set; } = string.Empty;
    
    // Output
    public string KodeOutput { get; set; } = string.Empty;
    public string NamaOutput { get; set; } = string.Empty;
    
    // Suboutput
    public string KodeSuboutput { get; set; } = string.Empty;
    public string NamaSuboutput { get; set; } = string.Empty;
    
    // Komponen
    public string KodeKomponen { get; set; } = string.Empty;
    public string NamaKomponen { get; set; } = string.Empty;
    
    // Subkomponen
    public string KodeSubkomponen { get; set; } = string.Empty;
    public string NamaSubkomponen { get; set; } = string.Empty;
    
    // Akun
    public string KodeAkun { get; set; } = string.Empty;
    public string NamaAkun { get; set; } = string.Empty;
    
    // Item (Optional)
    public Guid? ItemId { get; set; }
    public string? NoItem { get; set; }
    public string? NamaItem { get; set; }
    
    // Transaction Details
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
    
    // Navigation properties
    public Stpb Stpb { get; set; } = null!;
    public Item? Item { get; set; }
}
