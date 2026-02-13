using FinApp.Domain.Entities;
using FinApp.Domain.Interfaces;
using FinApp.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace FinApp.Infrastructure.Repositories;

public class RoleMenuPermissionRepository : Repository<RoleMenuPermission>, IRoleMenuPermissionRepository
{
    public RoleMenuPermissionRepository(AppDbContext context) : base(context)
    {
    }

    public async Task<List<RoleMenuPermission>> GetByRoleIdAsync(Guid roleId)
    {
        return await _dbSet
            .Include(rmp => rmp.Menu)
            .Where(rmp => rmp.RoleId == roleId && rmp.IsVisible)
            .ToListAsync();
    }

    public async Task<List<string>> GetMenuKeysByRoleIdAsync(Guid roleId)
    {
        return await _dbSet
            .Where(rmp => rmp.RoleId == roleId && rmp.IsVisible)
            .Select(rmp => rmp.MenuKey)
            .ToListAsync();
    }

    public async Task DeleteByRoleIdAsync(Guid roleId)
    {
        var permissions = await _dbSet
            .Where(rmp => rmp.RoleId == roleId)
            .ToListAsync();

        _dbSet.RemoveRange(permissions);
    }
}
