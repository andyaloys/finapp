using FinApp.Domain.Entities;

namespace FinApp.Core.DTOs.PpkBendahara;

public class PpkBendaharaDto
{
    public Guid Id { get; set; }
    public string Nama { get; set; } = string.Empty;
    public string NIP { get; set; } = string.Empty;
    public JabatanType Jabatan { get; set; }
    public string JabatanName { get; set; } = string.Empty;
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}
