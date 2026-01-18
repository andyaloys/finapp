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

    public override async Task<Stpb?> GetByIdAsync(int id)
    {
        return await _dbSet
            .Include(s => s.Creator)
            .FirstOrDefaultAsync(s => s.Id == id);
    }

    public async Task<IEnumerable<Stpb>> GetByUserIdAsync(int userId)
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
            .Where(s => s.Status == status)
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
                s.Deskripsi!.Contains(searchTerm) ||
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
}
