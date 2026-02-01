using FinApp.Core.DTOs.Common;
using FinApp.Core.DTOs.Role;

namespace FinApp.Core.Interfaces;

public interface IRoleService
{
    Task<PagedResult<RoleDto>> GetAllAsync(int pageNumber, int pageSize, string? searchTerm);
    Task<List<RoleDto>> GetAllRolesAsync();
    Task<RoleDto?> GetByIdAsync(Guid id);
    Task<RoleDto> CreateAsync(CreateRoleDto dto);
    Task<RoleDto> UpdateAsync(Guid id, UpdateRoleDto dto);
    Task DeleteAsync(Guid id);
    Task<List<RoleSuboutputDto>> GetRoleSuboutputsAsync(Guid roleId);
    Task AssignSuboutputsAsync(Guid roleId, AssignSuboutputsDto dto);
}
