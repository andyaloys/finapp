using FinApp.Domain.Entities;
using FinApp.Domain.Interfaces;
using FinApp.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace FinApp.Infrastructure.Repositories;

public class AnggaranMasterRepository : Repository<AnggaranMaster>, IAnggaranMasterRepository
{
    public AnggaranMasterRepository(AppDbContext context) : base(context)
    {
    }

    public async Task<List<AnggaranMaster>> GetByTahunRevisiAsync(int tahun, int revisi)
    {
        return await _context.AnggaranMasters
            .Where(x => x.TahunAnggaran == tahun && x.Revisi == revisi)
            .ToListAsync();
    }

    public async Task<int> GetLastRevisiAsync(int tahun)
    {
        return await _context.AnggaranMasters
            .Where(x => x.TahunAnggaran == tahun)
            .Select(x => (int?)x.Revisi)
            .MaxAsync() ?? -1;
    }

    public async Task<AnggaranMaster?> GetAnggaranByKeysAsync(
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
        return await _context.AnggaranMasters
            .FirstOrDefaultAsync(a =>
                a.TahunAnggaran == tahun &&
                a.Revisi == revisi &&
                a.KdProgram == kdProgram &&
                a.KdGiat == kdGiat &&
                a.KdOutput == kdOutput &&
                a.KdSOutput == kdSOutput &&
                a.KdKmpnen == kdKmpnen &&
                a.KdSkmpnen == kdSkmpnen &&
                a.KdAkun == kdAkun &&
                a.NoItem == noItem);
    }
}
