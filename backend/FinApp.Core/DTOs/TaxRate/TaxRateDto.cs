using FinApp.Domain.Entities;

namespace FinApp.Core.DTOs.TaxRate;

public class TaxRateDto
{
    public Guid Id { get; set; }
    public TaxType TaxType { get; set; }
    public string TaxTypeName { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public decimal Rate { get; set; }
    public string? Description { get; set; }
    public string? ReferenceCode { get; set; }
    public bool IsDefault { get; set; }
    public bool IsActive { get; set; }
    public int DisplayOrder { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}
