using FinApp.Domain.Entities;
using FinApp.Domain.Interfaces;
using FinApp.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace FinApp.Infrastructure.Repositories;

public class ProgramRepository : Repository<Program>, IProgramRepository
{
    public ProgramRepository(AppDbContext context) : base(context) { }

    public async Task<Program?> GetByKodeProgramAsync(string kodeProgram)
    {
        return await _context.Set<Program>()
            .FirstOrDefaultAsync(p => p.Kode == kodeProgram);
    }
}
