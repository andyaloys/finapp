namespace FinApp.Core.DTOs.Subkomponen
{
    public class SubkomponenDto
    {
        public Guid Id { get; set; }
        public string Kode { get; set; } = string.Empty;
        public string Nama { get; set; } = string.Empty;
        public Guid KomponenId { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
