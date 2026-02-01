using FinApp.Domain.Entities;

namespace FinApp.Domain.Interfaces;

public interface IRoleSuboutputRepository : IRepository<RoleSuboutput>
{
    Task<List<RoleSuboutput>> GetByRoleIdAsync(Guid roleId);
    Task DeleteByRoleIdAsync(Guid roleId);
}
