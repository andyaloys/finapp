namespace FinApp.Core.DTOs.Program
{
    public class UpdateProgramDto
    {
        public string Kode { get; set; } = string.Empty;
        public string Nama { get; set; } = string.Empty;
        public bool IsActive { get; set; }
    }
}
