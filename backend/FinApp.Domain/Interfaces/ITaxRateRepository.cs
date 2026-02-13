using FinApp.Domain.Entities;

namespace FinApp.Domain.Interfaces;

public interface ITaxRateRepository
{
    Task<TaxRate?> GetByIdAsync(int id);
    Task<TaxRate?> GetByCodeAsync(string taxCode);
    Task<IEnumerable<TaxRate>> GetAllAsync();
    Task<IEnumerable<TaxRate>> GetAllActiveAsync();
    Task<bool> ExistsByCodeAsync(string taxCode, int? excludeId = null);
    Task AddAsync(TaxRate taxRate);
    void Update(TaxRate taxRate);
    void Delete(TaxRate taxRate);
    Task<bool> SaveChangesAsync();
}
