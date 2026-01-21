using FinApp.Domain.Entities;
using FinApp.Domain.Interfaces;
using FinApp.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace FinApp.Infrastructure.Repositories;

public class ItemRepository : Repository<Item>, IItemRepository
{
    public ItemRepository(AppDbContext context) : base(context) { }

    public async Task<(IEnumerable<Item> Items, int TotalCount)> GetPagedAsync(int pageNumber, int pageSize, string? searchTerm)
    {
        var query = _context.Set<Item>()
            .Include(i => i.Akun)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(searchTerm))
        {
            query = query.Where(i => i.Nama.Contains(searchTerm) || i.Satuan.Contains(searchTerm));
        }

        var totalCount = await query.CountAsync();
        var items = await query
            .OrderBy(i => i.Nama)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return (items, totalCount);
    }

    public async Task<IEnumerable<Item>> GetByAkunIdAsync(Guid akunId)
    {
        return await _context.Set<Item>()
            .Include(i => i.Akun)
            .Where(i => i.AkunId == akunId && i.IsActive)
            .OrderBy(i => i.Nama)
            .ToListAsync();
    }
}
