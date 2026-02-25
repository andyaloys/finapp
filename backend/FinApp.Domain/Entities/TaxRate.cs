namespace FinApp.Domain.Entities;

public class TaxRate : BaseEntity
{
    public TaxType TaxType { get; set; }
    public string Category { get; set; } = string.Empty;
    public decimal Rate { get; set; }
    public string? Description { get; set; }
    public string? ReferenceCode { get; set; }
    public bool IsDefault { get; set; }
    public bool IsActive { get; set; } = true;
    public int DisplayOrder { get; set; }
}
