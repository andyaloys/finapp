namespace FinApp.Core.DTOs.Akun
{
    public class UpdateAkunDto
    {
        public string Kode { get; set; } = string.Empty;
        public string Nama { get; set; } = string.Empty;
        public Guid SubkomponenId { get; set; }
        public bool IsActive { get; set; }
    }
}
