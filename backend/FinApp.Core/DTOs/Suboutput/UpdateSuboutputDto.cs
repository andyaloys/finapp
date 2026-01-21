namespace FinApp.Core.DTOs.Suboutput
{
    public class UpdateSuboutputDto
    {
        public string Kode { get; set; } = string.Empty;
        public string Nama { get; set; } = string.Empty;
        public Guid OutputId { get; set; }
        public bool IsActive { get; set; }
    }
}
