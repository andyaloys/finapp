namespace FinApp.Domain.Entities;

public class Stpb : BaseEntity
{
    // Reference fields
    public string KodeProgram { get; set; } = string.Empty;
    public string KodeKegiatan { get; set; } = string.Empty;
    public string KodeOutput { get; set; } = string.Empty;
    public string KodeSuboutput { get; set; } = string.Empty;
    public string KodeKomponen { get; set; } = string.Empty;
    public string KodeSubkomponen { get; set; } = string.Empty;
    public string KodeAkun { get; set; } = string.Empty;
    public Guid? ItemId { get; set; }
    
    // Data fields
    public DateTime Tanggal { get; set; }
    public string Uraian { get; set; } = string.Empty;
    public decimal Nominal { get; set; }
    public decimal PPn { get; set; }
    public decimal PPh21 { get; set; }
    public decimal PPh22 { get; set; }
    public decimal PPh23 { get; set; }
    public decimal NilaiBersih { get; set; }
    public string NomorSTPB { get; set; } = string.Empty;
    public bool IsLocked { get; set; } = false;
    
    public Guid CreatedBy { get; set; }

    // Navigation properties
    public Item? Item { get; set; }
    public User? Creator { get; set; }
}
