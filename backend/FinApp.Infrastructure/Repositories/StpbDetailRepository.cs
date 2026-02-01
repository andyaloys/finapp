using FinApp.Domain.Entities;
using FinApp.Domain.Interfaces;
using FinApp.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace FinApp.Infrastructure.Repositories;

public class StpbDetailRepository : Repository<StpbDetail>, IStpbDetailRepository
{
    public StpbDetailRepository(AppDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<StpbDetail>> GetByStpbIdAsync(Guid stpbId)
    {
        return await _dbSet
            .Include(d => d.Item)
            .Where(d => d.StpbId == stpbId)
            .OrderBy(d => d.CreatedAt)
            .ToListAsync();
    }

    public async Task<IEnumerable<StpbDetail>> GetBySuboutputAsync(string kodeSuboutput)
    {
        return await _dbSet
            .Include(d => d.Stpb)
            .Where(d => d.KodeSuboutput == kodeSuboutput)
            .OrderByDescending(d => d.CreatedAt)
            .ToListAsync();
    }
}
