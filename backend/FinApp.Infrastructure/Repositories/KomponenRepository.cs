using FinApp.Domain.Entities;
using FinApp.Domain.Interfaces;
using FinApp.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace FinApp.Infrastructure.Repositories;

public class KomponenRepository : Repository<Komponen>, IKomponenRepository
{
    public KomponenRepository(AppDbContext context) : base(context) { }

    public async Task<(IEnumerable<Komponen> Items, int TotalCount)> GetPagedAsync(int pageNumber, int pageSize, string? searchTerm)
    {
        var query = _context.Set<Komponen>().AsQueryable();

        if (!string.IsNullOrWhiteSpace(searchTerm))
        {
            query = query.Where(k => k.Kode.Contains(searchTerm) || k.Nama.Contains(searchTerm));
        }

        var totalCount = await query.CountAsync();
        var items = await query
            .OrderBy(k => k.Kode)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return (items, totalCount);
    }

    public async Task<IEnumerable<Komponen>> GetBySuboutputIdAsync(Guid suboutputId)
    {
        return await _context.Set<Komponen>()
            .Where(k => k.SuboutputId == suboutputId && k.IsActive)
            .OrderBy(k => k.Kode)
            .ToListAsync();
    }
}
