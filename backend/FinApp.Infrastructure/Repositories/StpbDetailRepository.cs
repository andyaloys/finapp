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

    public async Task<decimal> GetRealisasiByItemAsync(
        int tahun, 
        int revisi, 
        string kdProgram, 
        string kdGiat, 
        string kdOutput, 
        string kdSOutput, 
        string kdKmpnen, 
        string kdSkmpnen, 
        string kdAkun, 
        string noItem)
    {
        return await _dbSet
            .Include(d => d.Stpb)
            .Where(d =>
                d.Stpb.Tahun == tahun &&
                d.KodeProgram == kdProgram &&
                d.KodeKegiatan == kdGiat &&
                d.KodeOutput == kdOutput &&
                d.KodeSuboutput == kdSOutput &&
                d.KodeKomponen == kdKmpnen &&
                d.KodeSubkomponen == kdSkmpnen &&
                d.KodeAkun == kdAkun &&
                d.NoItem == noItem &&
                d.Stpb.Status == StpbStatus.Approve)
            .SumAsync(d => d.JumlahHarga);
    }
}
