using FinApp.Domain.Entities;
using FinApp.Domain.Interfaces;
using FinApp.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace FinApp.Infrastructure.Repositories;

public class PpkBendaharaRepository : Repository<PpkBendahara>, IPpkBendaharaRepository
{
    public PpkBendaharaRepository(AppDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<PpkBendahara>> GetActiveAsync()
    {
        return await _dbSet
            .Where(p => p.IsActive)
            .OrderBy(p => p.Nama)
            .ToListAsync();
    }

    public async Task<PpkBendahara?> GetByNIPAsync(string nip)
    {
        return await _dbSet
            .FirstOrDefaultAsync(p => p.NIP == nip);
    }

    public async Task<IEnumerable<PpkBendahara>> GetByJabatanAsync(JabatanType jabatan)
    {
        return await _dbSet
            .Where(p => p.Jabatan == jabatan && p.IsActive)
            .OrderBy(p => p.Nama)
            .ToListAsync();
    }
}
