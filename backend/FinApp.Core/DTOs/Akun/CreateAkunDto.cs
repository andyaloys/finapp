namespace FinApp.Core.DTOs.Akun
{
    public class CreateAkunDto
    {
        public string Kode { get; set; } = string.Empty;
        public string Nama { get; set; } = string.Empty;
        public Guid SubkomponenId { get; set; }
    }
}
