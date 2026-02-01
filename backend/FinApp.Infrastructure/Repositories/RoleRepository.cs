using FinApp.Domain.Entities;
using FinApp.Domain.Interfaces;
using FinApp.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace FinApp.Infrastructure.Repositories;

public class RoleRepository : Repository<Role>, IRoleRepository
{
    public RoleRepository(AppDbContext context) : base(context)
    {
    }

    public async Task<Role?> GetByNameAsync(string name)
    {
        return await _dbSet.FirstOrDefaultAsync(r => r.Name == name);
    }

    public async Task<Role?> GetByIdWithSuboutputsAsync(Guid id)
    {
        return await _dbSet
            .Include(r => r.RoleSuboutputs)
            .FirstOrDefaultAsync(r => r.Id == id);
    }

    public async Task<(IEnumerable<Role> items, int totalCount)> GetPagedAsync(int pageNumber, int pageSize, string? searchTerm)
    {
        var query = _dbSet.AsQueryable();

        if (!string.IsNullOrWhiteSpace(searchTerm))
        {
            query = query.Where(r => r.Name.Contains(searchTerm) || 
                                   (r.Description != null && r.Description.Contains(searchTerm)));
        }

        var totalCount = await query.CountAsync();
        var items = await query
            .OrderBy(r => r.Name)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return (items, totalCount);
    }
}
