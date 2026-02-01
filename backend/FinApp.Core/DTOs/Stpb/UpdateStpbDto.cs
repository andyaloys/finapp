namespace FinApp.Core.DTOs.Stpb;

public class UpdateStpbDto
{
    public DateTime TanggalSTPB { get; set; }
    public Guid PpkBendaharaId { get; set; }
    public string? Keterangan { get; set; }
}
