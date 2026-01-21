namespace FinApp.Core.DTOs.Item
{
    public class ItemDto
    {
        public Guid Id { get; set; }
        public string nama { get; set; } = string.Empty;
        public string satuan { get; set; } = string.Empty;
        public decimal hargaSatuan { get; set; }
        public Guid akunId { get; set; }
        public string kodeAkun { get; set; } = string.Empty;
        public bool isActive { get; set; }
        public DateTime createdAt { get; set; }
    }
}
