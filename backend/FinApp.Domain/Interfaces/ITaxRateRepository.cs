using FinApp.Domain.Entities;

namespace FinApp.Domain.Interfaces;

public interface ITaxRateRepository : IRepository<TaxRate>
{
    Task<IEnumerable<TaxRate>> GetByTaxTypeAsync(TaxType taxType);
    Task<TaxRate?> GetDefaultByTaxTypeAsync(TaxType taxType);
    Task<IEnumerable<TaxRate>> GetActiveTaxRatesAsync();
}
