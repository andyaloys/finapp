namespace FinApp.Core.DTOs.Subkomponen
{
    public class CreateSubkomponenDto
    {
        public string Kode { get; set; } = string.Empty;
        public string Nama { get; set; } = string.Empty;
        public Guid KomponenId { get; set; }
    }
}
