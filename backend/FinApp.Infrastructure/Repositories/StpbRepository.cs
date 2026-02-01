using FinApp.Domain.Entities;
using FinApp.Domain.Interfaces;
using FinApp.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace FinApp.Infrastructure.Repositories;

public class StpbRepository : Repository<Stpb>, IStpbRepository
{
    public StpbRepository(AppDbContext context) : base(context)
    {
    }

    public override async Task<Stpb?> GetByIdAsync(Guid id)
    {
        return await _dbSet
            .Include(s => s.Creator)
            .Include(s => s.PpkBendahara)
            .Include(s => s.StpbDetails)
            .FirstOrDefaultAsync(s => s.Id == id);
    }

    public async Task<IEnumerable<Stpb>> GetByUserIdAsync(Guid userId)
    {
        return await _dbSet
            .Include(s => s.Creator)
            .Include(s => s.PpkBendahara)
            .Include(s => s.StpbDetails)
            .Where(s => s.CreatedBy == userId)
            .OrderByDescending(s => s.CreatedAt)
            .ToListAsync();
    }

    public async Task<Stpb?> GetByNomorAsync(string nomorStpb)
    {
        return await _dbSet
            .Include(s => s.Creator)
            .Include(s => s.PpkBendahara)
            .Include(s => s.StpbDetails)
            .FirstOrDefaultAsync(s => s.NomorSTPB == nomorStpb);
    }

    public async Task<IEnumerable<Stpb>> GetByStatusAsync(string status)
    {
        // Convert status string to enum
        if (!Enum.TryParse<StpbStatus>(status, out var statusEnum))
            return Enumerable.Empty<Stpb>();

        return await _dbSet
            .Include(s => s.Creator)
            .Include(s => s.PpkBendahara)
            .Include(s => s.StpbDetails)
            .Where(s => s.Status == statusEnum)
            .OrderByDescending(s => s.CreatedAt)
            .ToListAsync();
    }

    public async Task<(IEnumerable<Stpb> Items, int TotalCount)> GetPagedAsync(
        int pageNumber, 
        int pageSize, 
        string? searchTerm = null)
    {
        var query = _dbSet
            .Include(s => s.Creator)
            .Include(s => s.PpkBendahara)
            .Include(s => s.StpbDetails)
            .AsQueryable();

        // Apply search filter
        if (!string.IsNullOrWhiteSpace(searchTerm))
        {
            query = query.Where(s => 
                s.NomorSTPB.Contains(searchTerm) ||
                (s.Keterangan != null && s.Keterangan.Contains(searchTerm)) ||
                s.Creator!.FullName.Contains(searchTerm) ||
                s.PpkBendahara.Nama.Contains(searchTerm)
            );
        }

        var totalCount = await query.CountAsync();

        var items = await query
            .OrderByDescending(s => s.CreatedAt)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return (items, totalCount);
    }

    public async Task<int> GetLastNumberByYearAsync(int year)
    {
        // No longer needed - SequenceNumberRepository handles this
        // Kept for backward compatibility
        return 0;
    }
}
