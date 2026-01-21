using FinApp.Domain.Entities;
using FinApp.Domain.Interfaces;
using FinApp.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace FinApp.Infrastructure.Repositories;

public class OutputRepository : Repository<Output>, IOutputRepository
{
    public OutputRepository(AppDbContext context) : base(context) { }

    public async Task<(IEnumerable<Output> Items, int TotalCount)> GetPagedAsync(int pageNumber, int pageSize, string? searchTerm)
    {
        var query = _context.Set<Output>().AsQueryable();

        if (!string.IsNullOrWhiteSpace(searchTerm))
        {
            query = query.Where(o => o.Kode.Contains(searchTerm) || o.Nama.Contains(searchTerm));
        }

        var totalCount = await query.CountAsync();
        var items = await query
            .OrderBy(o => o.Kode)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return (items, totalCount);
    }

    public async Task<IEnumerable<Output>> GetByKegiatanIdAsync(Guid kegiatanId)
    {
        return await _context.Set<Output>()
            .Where(o => o.KegiatanId == kegiatanId && o.IsActive)
            .OrderBy(o => o.Kode)
            .ToListAsync();
    }
}
