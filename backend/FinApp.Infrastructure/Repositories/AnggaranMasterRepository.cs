using FinApp.Domain.Entities;
using FinApp.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace FinApp.Infrastructure.Repositories;

public class AnggaranMasterRepository
{
    private readonly AppDbContext _context;
    public AnggaranMasterRepository(AppDbContext context)
    {
        _context = context;
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
}
