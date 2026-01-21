using FinApp.Domain.Entities;
using FinApp.Domain.Interfaces;
using FinApp.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace FinApp.Infrastructure.Repositories;

public class KegiatanRepository : Repository<Kegiatan>, IKegiatanRepository
{
    public KegiatanRepository(AppDbContext context) : base(context) { }

    public async Task<(IEnumerable<Kegiatan> Items, int TotalCount)> GetPagedAsync(int pageNumber, int pageSize, string? searchTerm)
    {
        var query = _context.Set<Kegiatan>().AsQueryable();

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

    public async Task<IEnumerable<Kegiatan>> GetByProgramIdAsync(Guid programId)
    {
        return await _context.Set<Kegiatan>()
            .Where(k => k.ProgramId == programId && k.IsActive)
            .OrderBy(k => k.Kode)
            .ToListAsync();
    }
}
