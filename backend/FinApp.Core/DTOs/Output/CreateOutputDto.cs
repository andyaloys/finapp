namespace FinApp.Core.DTOs.Output
{
    public class CreateOutputDto
    {
        public string Kode { get; set; } = string.Empty;
        public string Nama { get; set; } = string.Empty;
        public Guid KegiatanId { get; set; }
    }
}
