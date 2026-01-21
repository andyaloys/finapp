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
            .FirstOrDefaultAsync(s => s.Id == id);
    }

    public async Task<IEnumerable<Stpb>> GetByUserIdAsync(Guid userId)
    {
        return await _dbSet
            .Include(s => s.Creator)
            .Where(s => s.CreatedBy == userId)
            .OrderByDescending(s => s.CreatedAt)
            .ToListAsync();
    }

    public async Task<Stpb?> GetByNomorAsync(string nomorStpb)
    {
        return await _dbSet
            .Include(s => s.Creator)
            .FirstOrDefaultAsync(s => s.NomorSTPB == nomorStpb);
    }

    public async Task<IEnumerable<Stpb>> GetByStatusAsync(string status)
    {
        return await _dbSet
            .Include(s => s.Creator)
            .Where(s => s.IsLocked == (status == "Locked"))
            .OrderByDescending(s => s.CreatedAt)
            .ToListAsync();
    }

    public async Task<(IEnumerable<Stpb> Items, int TotalCount)> GetPagedAsync(
        int pageNumber, 
        int pageSize, 
        string? searchTerm = null)
    {
        var query = _dbSet.Include(s => s.Creator).AsQueryable();

        // Apply search filter
        if (!string.IsNullOrWhiteSpace(searchTerm))
        {
            query = query.Where(s => 
                s.NomorSTPB.Contains(searchTerm) ||
                s.Uraian!.Contains(searchTerm) ||
                s.Creator!.FullName.Contains(searchTerm)
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
        // Get the last STPB number for the given year
        var lastStpb = await _dbSet
            .Where(s => s.Tanggal.Year == year && !string.IsNullOrEmpty(s.NomorSTPB))
            .OrderByDescending(s => s.CreatedAt)
            .FirstOrDefaultAsync();

        if (lastStpb == null || string.IsNullOrEmpty(lastStpb.NomorSTPB))
            return 0;

        // Parse number from format "STPB-XXX/YYYY"
        var parts = lastStpb.NomorSTPB.Split('-', '/');
        if (parts.Length >= 2 && int.TryParse(parts[1], out int number))
            return number;

        return 0;
    }
}
