using FinApp.Domain.Entities;
using FinApp.Domain.Interfaces;
using FinApp.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace FinApp.Infrastructure.Repositories;

public class SuboutputRepository : Repository<Suboutput>, ISuboutputRepository
{
    public SuboutputRepository(AppDbContext context) : base(context) { }

    public async Task<(IEnumerable<Suboutput> Items, int TotalCount)> GetPagedAsync(int pageNumber, int pageSize, string? searchTerm)
    {
        var query = _context.Set<Suboutput>().AsQueryable();

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

    public async Task<IEnumerable<Suboutput>> GetByOutputIdAsync(Guid outputId)
    {
        return await _context.Set<Suboutput>()
            .Where(s => s.OutputId == outputId && s.IsActive)
            .OrderBy(s => s.Kode)
            .ToListAsync();
    }
}
