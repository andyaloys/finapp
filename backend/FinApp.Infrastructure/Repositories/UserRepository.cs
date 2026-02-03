using FinApp.Domain.Entities;
using FinApp.Domain.Interfaces;
using FinApp.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace FinApp.Infrastructure.Repositories;

public class UserRepository : Repository<User>, IUserRepository
{
    public UserRepository(AppDbContext context) : base(context)
    {
    }

    public override async Task<IEnumerable<User>> GetAllAsync()
    {
        return await _dbSet
            .Include(u => u.Role)
            .Include(u => u.PpkBendahara)
            .ToListAsync();
    }

    public async Task<User?> GetByUsernameAsync(string username)
    {
        return await _dbSet.FirstOrDefaultAsync(u => u.Username == username);
    }

    public async Task<User?> GetByUsernameWithRoleAsync(string username)
    {
        return await _dbSet
            .Include(u => u.Role)
                .ThenInclude(r => r.RoleSuboutputs)
            .Include(u => u.PpkBendahara)
            .FirstOrDefaultAsync(u => u.Username == username);
    }

    public async Task<User?> GetByIdWithRoleAsync(Guid id)
    {
        return await _dbSet
            .Include(u => u.Role)
                .ThenInclude(r => r.RoleSuboutputs)
            .Include(u => u.PpkBendahara)
            .FirstOrDefaultAsync(u => u.Id == id);
    }

    public async Task<User?> GetByEmailAsync(string email)
    {
        return await _dbSet.FirstOrDefaultAsync(u => u.Email == email);
    }

    public async Task<bool> UsernameExistsAsync(string username)
    {
        return await _dbSet.AnyAsync(u => u.Username == username);
    }
}
