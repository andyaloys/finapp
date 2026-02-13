using FinApp.Domain.Entities;
using FinApp.Domain.Interfaces;
using FinApp.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace FinApp.Infrastructure.Repositories;

public class TaxRateRepository : ITaxRateRepository
{
    private readonly AppDbContext _context;

    public TaxRateRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<TaxRate?> GetByIdAsync(int id)
    {
        return await _context.TaxRates.FindAsync(id);
    }

    public async Task<TaxRate?> GetByCodeAsync(string taxCode)
    {
        return await _context.TaxRates
            .FirstOrDefaultAsync(t => t.TaxCode == taxCode);
    }

    public async Task<IEnumerable<TaxRate>> GetAllAsync()
    {
        return await _context.TaxRates
            .OrderBy(t => t.TaxCode)
            .ToListAsync();
    }

    public async Task<IEnumerable<TaxRate>> GetAllActiveAsync()
    {
        return await _context.TaxRates
            .Where(t => t.IsActive)
            .OrderBy(t => t.TaxCode)
            .ToListAsync();
    }

    public async Task<bool> ExistsByCodeAsync(string taxCode, int? excludeId = null)
    {
        var query = _context.TaxRates.Where(t => t.TaxCode == taxCode);
        
        if (excludeId.HasValue)
        {
            query = query.Where(t => t.Id != excludeId.Value);
        }
        
        return await query.AnyAsync();
    }

    public async Task AddAsync(TaxRate taxRate)
    {
        await _context.TaxRates.AddAsync(taxRate);
    }

    public void Update(TaxRate taxRate)
    {
        _context.TaxRates.Update(taxRate);
    }

    public void Delete(TaxRate taxRate)
    {
        _context.TaxRates.Remove(taxRate);
    }

    public async Task<bool> SaveChangesAsync()
    {
        return await _context.SaveChangesAsync() > 0;
    }
}
