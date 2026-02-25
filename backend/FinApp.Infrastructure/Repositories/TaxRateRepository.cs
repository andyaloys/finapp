using FinApp.Domain.Entities;
using FinApp.Domain.Interfaces;
using FinApp.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace FinApp.Infrastructure.Repositories;

public class TaxRateRepository : Repository<TaxRate>, ITaxRateRepository
{
    public TaxRateRepository(AppDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<TaxRate>> GetByTaxTypeAsync(TaxType taxType)
    {
        return await _dbSet
            .Where(tr => tr.TaxType == taxType && tr.IsActive)
            .OrderBy(tr => tr.DisplayOrder)
            .ToListAsync();
    }

    public async Task<TaxRate?> GetDefaultByTaxTypeAsync(TaxType taxType)
    {
        return await _dbSet
            .FirstOrDefaultAsync(tr => tr.TaxType == taxType && tr.IsDefault && tr.IsActive);
    }

    public async Task<IEnumerable<TaxRate>> GetActiveTaxRatesAsync()
    {
        return await _dbSet
            .Where(tr => tr.IsActive)
            .OrderBy(tr => tr.TaxType)
            .ThenBy(tr => tr.DisplayOrder)
            .ToListAsync();
    }
}
