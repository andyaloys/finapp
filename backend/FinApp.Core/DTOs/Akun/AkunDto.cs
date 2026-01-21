namespace FinApp.Core.DTOs.Akun
{
    public class AkunDto
    {
        public Guid Id { get; set; }
        public string Kode { get; set; } = string.Empty;
        public string Nama { get; set; } = string.Empty;
        public Guid SubkomponenId { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
