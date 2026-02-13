using AutoMapper;
using FinApp.Core.DTOs.Common;
using FinApp.Core.DTOs.Menu;
using FinApp.Core.Interfaces;
using FinApp.Domain.Entities;
using FinApp.Domain.Interfaces;

namespace FinApp.Core.Services;

public class MenuService : IMenuService
{
    private readonly IMenuRepository _menuRepository;
    private readonly IRoleMenuPermissionRepository _roleMenuPermissionRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public MenuService(
        IMenuRepository menuRepository,
        IRoleMenuPermissionRepository roleMenuPermissionRepository,
        IUnitOfWork unitOfWork,
        IMapper mapper)
    {
        _menuRepository = menuRepository;
        _roleMenuPermissionRepository = roleMenuPermissionRepository;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<ApiResponse<List<MenuDto>>> GetAllMenusAsync()
    {
        try
        {
            var menus = await _menuRepository.GetAllActiveAsync();
            var menuDtos = _mapper.Map<List<MenuDto>>(menus);
            return ApiResponse<List<MenuDto>>.SuccessResponse(menuDtos, "Menus retrieved successfully");
        }
        catch (Exception ex)
        {
            return ApiResponse<List<MenuDto>>.ErrorResponse($"Error retrieving menus: {ex.Message}");
        }
    }

    public async Task<ApiResponse<List<string>>> GetRoleMenuPermissionsAsync(Guid roleId)
    {
        try
        {
            var menuKeys = await _roleMenuPermissionRepository.GetMenuKeysByRoleIdAsync(roleId);
            return ApiResponse<List<string>>.SuccessResponse(menuKeys, "Permissions retrieved successfully");
        }
        catch (Exception ex)
        {
            return ApiResponse<List<string>>.ErrorResponse($"Error retrieving permissions: {ex.Message}");
        }
    }

    public async Task<ApiResponse<bool>> UpdateRoleMenuPermissionsAsync(Guid roleId, List<string> menuKeys)
    {
        try
        {
            // Delete existing permissions
            await _roleMenuPermissionRepository.DeleteByRoleIdAsync(roleId);

            // Add new permissions
            foreach (var menuKey in menuKeys)
            {
                // Get menu entity by key
                var menu = await _menuRepository.GetByKeyAsync(menuKey);
                if (menu == null) continue; // Skip if menu not found
                
                var permission = new RoleMenuPermission
                {
                    RoleId = roleId,
                    MenuKey = menuKey,
                    Menu = menu,
                    IsVisible = true,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                };
                await _roleMenuPermissionRepository.AddAsync(permission);
            }

            await _unitOfWork.SaveChangesAsync();
            return ApiResponse<bool>.SuccessResponse(true, "Permissions updated successfully");
        }
        catch (Exception ex)
        {
            return ApiResponse<bool>.ErrorResponse($"Error updating permissions: {ex.Message}");
        }
    }
}
