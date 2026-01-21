namespace FinApp.Core.DTOs.Item
{
    public class CreateItemDto
    {
        public string nama { get; set; } = string.Empty;
        public string satuan { get; set; } = string.Empty;
        public decimal hargaSatuan { get; set; }
        public Guid akunId { get; set; }
        public bool isActive { get; set; } = true;
    }
}
