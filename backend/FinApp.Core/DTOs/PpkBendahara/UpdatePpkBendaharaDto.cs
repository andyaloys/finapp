namespace FinApp.Core.DTOs.PpkBendahara;

public class UpdatePpkBendaharaDto
{
    public string Nama { get; set; } = string.Empty;
    public string NIP { get; set; } = string.Empty;
    public int Jabatan { get; set; } // 1=PPK, 2=Bendahara
    public bool IsActive { get; set; }
}
