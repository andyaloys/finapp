namespace FinApp.Core.DTOs.Komponen
{
    public class CreateKomponenDto
    {
        public string Kode { get; set; } = string.Empty;
        public string Nama { get; set; } = string.Empty;
        public Guid SuboutputId { get; set; }
    }
}
