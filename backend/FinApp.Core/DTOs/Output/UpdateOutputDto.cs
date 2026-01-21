namespace FinApp.Core.DTOs.Output
{
    public class UpdateOutputDto
    {
        public string Kode { get; set; } = string.Empty;
        public string Nama { get; set; } = string.Empty;
        public Guid KegiatanId { get; set; }
        public bool IsActive { get; set; }
    }
}
