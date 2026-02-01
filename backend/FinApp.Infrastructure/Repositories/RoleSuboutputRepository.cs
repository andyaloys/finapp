using FinApp.Domain.Entities;
using FinApp.Domain.Interfaces;
using FinApp.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace FinApp.Infrastructure.Repositories;

public class RoleSuboutputRepository : Repository<RoleSuboutput>, IRoleSuboutputRepository
{
    public RoleSuboutputRepository(AppDbContext context) : base(context)
    {
    }

    public async Task<List<RoleSuboutput>> GetByRoleIdAsync(Guid roleId)
    {
        return await _dbSet
            .Where(rs => rs.RoleId == roleId)
            .ToListAsync();
    }

    public async Task DeleteByRoleIdAsync(Guid roleId)
    {
        var items = await _dbSet.Where(rs => rs.RoleId == roleId).ToListAsync();
        _dbSet.RemoveRange(items);
    }
}
