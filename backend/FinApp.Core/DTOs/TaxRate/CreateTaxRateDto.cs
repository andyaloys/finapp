namespace FinApp.Core.DTOs.TaxRate;

public class CreateTaxRateDto
{
    public string TaxCode { get; set; } = string.Empty;
    public string TaxName { get; set; } = string.Empty;
    public decimal Rate { get; set; }
}
