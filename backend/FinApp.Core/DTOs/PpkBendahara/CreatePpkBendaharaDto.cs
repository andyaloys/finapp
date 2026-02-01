using FinApp.Domain.Entities;

namespace FinApp.Core.DTOs.PpkBendahara;

public class CreatePpkBendaharaDto
{
    public string Nama { get; set; } = string.Empty;
    public string NIP { get; set; } = string.Empty;
    public JabatanType Jabatan { get; set; } // 1=PPK, 2=Bendahara
    public bool IsActive { get; set; } = true;
}
