using FinApp.Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FinApp.API.Controllers;

[Authorize]
public class MenuController : BaseApiController
{
    private readonly IMenuService _menuService;

    public MenuController(IMenuService menuService)
    {
        _menuService = menuService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllMenus()
    {
        var result = await _menuService.GetAllMenusAsync();
        return Ok(result);
    }

    [HttpGet("role/{roleId}/permissions")]
    public async Task<IActionResult> GetRolePermissions(Guid roleId)
    {
        var result = await _menuService.GetRoleMenuPermissionsAsync(roleId);
        return Ok(result);
    }

    [HttpPut("role/{roleId}/permissions")]
    public async Task<IActionResult> UpdateRolePermissions(Guid roleId, [FromBody] List<string> menuKeys)
    {
        var result = await _menuService.UpdateRoleMenuPermissionsAsync(roleId, menuKeys);
        return Ok(result);
    }

    [AllowAnonymous]
    [HttpPost("seed-admin-permissions")]
    public async Task<IActionResult> SeedAdminPermissions()
    {
        var adminRoleId = Guid.Parse("00000000-0000-0000-0000-000000000010");
        var allMenuKeys = new List<string> 
        { 
            "transaksi", "transaksi-stpb", 
            "anggaran", "anggaran-list", 
            "monitoring", 
            "master-data", "master-ppkbendahara", 
            "administration", "admin-users", "admin-roles" 
        };
        
        var result = await _menuService.UpdateRoleMenuPermissionsAsync(adminRoleId, allMenuKeys);
        return Ok(result);
    }
}
