using FinApp.Domain.Entities;
using FinApp.Domain.Interfaces;
using FinApp.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace FinApp.Infrastructure.Repositories;

public class MenuRepository : Repository<Menu>, IMenuRepository
{
    public MenuRepository(AppDbContext context) : base(context)
    {
    }

    public async Task<List<Menu>> GetAllActiveAsync()
    {
        return await _dbSet
            .Where(m => m.IsActive)
            .OrderBy(m => m.Order)
            .ToListAsync();
    }

    public async Task<Menu?> GetByKeyAsync(string key)
    {
        return await _dbSet.FirstOrDefaultAsync(m => m.Key == key);
    }
}
