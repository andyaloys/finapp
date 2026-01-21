using FinApp.Domain.Entities;
using FinApp.Domain.Interfaces;
using FinApp.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace FinApp.Infrastructure.Repositories;

public class SubkomponenRepository : Repository<Subkomponen>, ISubkomponenRepository
{
    public SubkomponenRepository(AppDbContext context) : base(context) { }

    public async Task<(IEnumerable<Subkomponen> Items, int TotalCount)> GetPagedAsync(int pageNumber, int pageSize, string? searchTerm)
    {
        var query = _context.Set<Subkomponen>().AsQueryable();

        if (!string.IsNullOrWhiteSpace(searchTerm))
        {
            query = query.Where(s => s.Kode.Contains(searchTerm) || s.Nama.Contains(searchTerm));
        }

        var totalCount = await query.CountAsync();
        var items = await query
            .OrderBy(s => s.Kode)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return (items, totalCount);
    }

    public async Task<IEnumerable<Subkomponen>> GetByKomponenIdAsync(Guid komponenId)
    {
        return await _context.Set<Subkomponen>()
            .Where(s => s.KomponenId == komponenId && s.IsActive)
            .OrderBy(s => s.Kode)
            .ToListAsync();
    }
}
