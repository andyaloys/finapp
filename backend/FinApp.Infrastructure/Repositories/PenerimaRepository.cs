using FinApp.Domain.Entities;
using FinApp.Domain.Interfaces;
using FinApp.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace FinApp.Infrastructure.Repositories;

public class PenerimaRepository : IPenerimaRepository
{
    private readonly AppDbContext _context;

    public PenerimaRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Penerima?> GetByIdAsync(int id)
    {
        return await _context.Penerimas
            .FirstOrDefaultAsync(p => p.Id == id);
    }

    public async Task<IEnumerable<Penerima>> GetAllAsync()
    {
        return await _context.Penerimas
            .OrderBy(p => p.Nama)
            .ToListAsync();
    }

    public async Task<IEnumerable<Penerima>> GetAllActiveAsync()
    {
        return await _context.Penerimas
            .Where(p => p.IsActive)
            .OrderBy(p => p.Nama)
            .ToListAsync();
    }

    public async Task<Penerima?> GetByNamaAsync(string nama)
    {
        return await _context.Penerimas
            .FirstOrDefaultAsync(p => p.Nama.ToLower() == nama.ToLower());
    }

    public async Task<bool> ExistsByNamaAsync(string nama, int? excludeId = null)
    {
        var query = _context.Penerimas
            .Where(p => p.Nama.ToLower() == nama.ToLower());

        if (excludeId.HasValue)
        {
            query = query.Where(p => p.Id != excludeId.Value);
        }

        return await query.AnyAsync();
    }

    public async Task AddAsync(Penerima penerima)
    {
        await _context.Penerimas.AddAsync(penerima);
    }

    public void Update(Penerima penerima)
    {
        _context.Penerimas.Update(penerima);
    }

    public void Delete(Penerima penerima)
    {
        _context.Penerimas.Remove(penerima);
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
}
