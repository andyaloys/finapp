namespace FinApp.Core.DTOs.Program
{
    public class ProgramDto
    {
        public Guid Id { get; set; }
        public string Kode { get; set; } = string.Empty;
        public string Nama { get; set; } = string.Empty;
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
