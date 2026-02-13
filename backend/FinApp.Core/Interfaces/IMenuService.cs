using FinApp.Core.DTOs.Common;
using FinApp.Core.DTOs.Menu;

namespace FinApp.Core.Interfaces;

public interface IMenuService
{
    Task<ApiResponse<List<MenuDto>>> GetAllMenusAsync();
    Task<ApiResponse<List<string>>> GetRoleMenuPermissionsAsync(Guid roleId);
    Task<ApiResponse<bool>> UpdateRoleMenuPermissionsAsync(Guid roleId, List<string> menuKeys);
}
