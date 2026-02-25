using FinApp.Domain.Entities;

namespace FinApp.Core.DTOs.TaxRate;

public class CreateTaxRateDto
{
    public TaxType TaxType { get; set; }
    public string Category { get; set; } = string.Empty;
    public decimal Rate { get; set; }
    public string? Description { get; set; }
    public string? ReferenceCode { get; set; }
    public bool IsDefault { get; set; }
    public int DisplayOrder { get; set; } = 1;
}
