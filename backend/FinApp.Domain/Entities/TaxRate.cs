namespace FinApp.Domain.Entities;

public class TaxRate : BaseEntity
{
    public new int Id { get; set; }
    public string TaxCode { get; set; } = string.Empty;
    public string TaxName { get; set; } = string.Empty;
    public decimal Rate { get; set; }
    public bool IsActive { get; set; } = true;
}
