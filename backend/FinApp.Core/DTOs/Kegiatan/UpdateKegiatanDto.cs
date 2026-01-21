namespace FinApp.Core.DTOs.Kegiatan
{
    public class UpdateKegiatanDto
    {
        public string Kode { get; set; } = string.Empty;
        public string Nama { get; set; } = string.Empty;
        public Guid ProgramId { get; set; }
        public bool IsActive { get; set; }
    }
}
