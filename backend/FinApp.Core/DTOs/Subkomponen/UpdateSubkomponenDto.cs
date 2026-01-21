namespace FinApp.Core.DTOs.Subkomponen
{
    public class UpdateSubkomponenDto
    {
        public string Kode { get; set; } = string.Empty;
        public string Nama { get; set; } = string.Empty;
        public Guid KomponenId { get; set; }
        public bool IsActive { get; set; }
    }
}
