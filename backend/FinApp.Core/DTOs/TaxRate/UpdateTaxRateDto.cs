namespace FinApp.Core.DTOs.TaxRate;

public class UpdateTaxRateDto
{
    public string TaxName { get; set; } = string.Empty;
    public decimal Rate { get; set; }
    public bool IsActive { get; set; }
}
