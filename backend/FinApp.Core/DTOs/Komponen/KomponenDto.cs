namespace FinApp.Core.DTOs.Komponen
{
    public class KomponenDto
    {
        public Guid Id { get; set; }
        public string Kode { get; set; } = string.Empty;
        public string Nama { get; set; } = string.Empty;
        public Guid SuboutputId { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
