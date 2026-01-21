namespace FinApp.Core.DTOs.Kegiatan
{
    public class KegiatanDto
    {
        public Guid Id { get; set; }
        public string Kode { get; set; } = string.Empty;
        public string Nama { get; set; } = string.Empty;
        public Guid ProgramId { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
