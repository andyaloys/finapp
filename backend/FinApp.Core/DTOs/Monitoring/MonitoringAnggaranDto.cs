namespace FinApp.Core.DTOs.Monitoring;

public class MonitoringAnggaranDto
{
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
    public string NoItem { get; set; } = string.Empty;
    public string NamaItem { get; set; } = string.Empty;
    
    // Financial data
    public decimal PaguAnggaran { get; set; }
    public decimal Realisasi { get; set; }
    public decimal SisaAnggaran { get; set; }
    public decimal PersenRealisasi { get; set; }
    
    // Additional info
    public int TahunAnggaran { get; set; }
    public int Revisi { get; set; }
    
    // Computed COA
    public string COA => $"{KodeProgram}.{KodeKegiatan}.{KodeOutput}.{KodeSuboutput}.{KodeKomponen}.{KodeSubkomponen}.{KodeAkun}.{NoItem}";
}
