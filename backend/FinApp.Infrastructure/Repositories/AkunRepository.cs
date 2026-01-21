using FinApp.Domain.Entities;
using FinApp.Domain.Interfaces;
using FinApp.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace FinApp.Infrastructure.Repositories;

public class AkunRepository : Repository<Akun>, IAkunRepository
{
    public AkunRepository(AppDbContext context) : base(context) { }

    public async Task<(IEnumerable<Akun> Items, int TotalCount)> GetPagedAsync(int pageNumber, int pageSize, string? searchTerm)
    {
        var query = _context.Set<Akun>().AsQueryable();

        if (!string.IsNullOrWhiteSpace(searchTerm))
        {
            query = query.Where(a => a.Kode.Contains(searchTerm) || a.Nama.Contains(searchTerm));
        }

        var totalCount = await query.CountAsync();
        var items = await query
            .OrderBy(a => a.Kode)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return (items, totalCount);
    }

    public async Task<IEnumerable<Akun>> GetBySubkomponenIdAsync(Guid subkomponenId)
    {
        return await _context.Set<Akun>()
            .Where(a => a.SubkomponenId == subkomponenId && a.IsActive)
            .OrderBy(a => a.Kode)
            .ToListAsync();
    }
}
