using FinApp.Domain.Entities;

namespace FinApp.Domain.Interfaces;

public interface IRoleMenuPermissionRepository : IRepository<RoleMenuPermission>
{
    Task<List<RoleMenuPermission>> GetByRoleIdAsync(Guid roleId);
    Task<List<string>> GetMenuKeysByRoleIdAsync(Guid roleId);
    Task DeleteByRoleIdAsync(Guid roleId);
}
