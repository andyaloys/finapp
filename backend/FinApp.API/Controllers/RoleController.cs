using FinApp.Core.DTOs.Common;
using FinApp.Core.DTOs.Role;
using FinApp.Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FinApp.API.Controllers;

[Authorize]
public class RoleController : BaseApiController
{
    private readonly IRoleService _roleService;
    private readonly ILogger<RoleController> _logger;

    public RoleController(IRoleService roleService, ILogger<RoleController> logger)
    {
        _roleService = roleService;
        _logger = logger;
    }

    [HttpGet]
    public async Task<ActionResult<ApiResponse<PagedResult<RoleDto>>>> GetAll(
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10,
        [FromQuery] string? searchTerm = null)
    {
        try
        {
            var result = await _roleService.GetAllAsync(pageNumber, pageSize, searchTerm);
            return Ok(ApiResponse<PagedResult<RoleDto>>.SuccessResponse(result));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting role list");
            return StatusCode(500, ApiResponse<PagedResult<RoleDto>>.ErrorResponse("Internal server error"));
        }
    }

    [HttpGet("all")]
    public async Task<ActionResult<ApiResponse<List<RoleDto>>>> GetAllRoles()
    {
        try
        {
            var result = await _roleService.GetAllRolesAsync();
            return Ok(ApiResponse<List<RoleDto>>.SuccessResponse(result));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting all roles");
            return StatusCode(500, ApiResponse<List<RoleDto>>.ErrorResponse("Internal server error"));
        }
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ApiResponse<RoleDto>>> GetById(Guid id)
    {
        try
        {
            var result = await _roleService.GetByIdAsync(id);
            
            if (result == null)
            {
                return NotFound(ApiResponse<RoleDto>.ErrorResponse($"Role with ID {id} not found"));
            }

            return Ok(ApiResponse<RoleDto>.SuccessResponse(result));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting role {Id}", id);
            return StatusCode(500, ApiResponse<RoleDto>.ErrorResponse("Internal server error"));
        }
    }

    [HttpPost]
    public async Task<ActionResult<ApiResponse<RoleDto>>> Create([FromBody] CreateRoleDto dto)
    {
        try
        {
            var result = await _roleService.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = result.Id }, ApiResponse<RoleDto>.SuccessResponse(result));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating role");
            return BadRequest(ApiResponse<RoleDto>.ErrorResponse(ex.Message));
        }
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<ApiResponse<RoleDto>>> Update(Guid id, [FromBody] UpdateRoleDto dto)
    {
        try
        {
            var result = await _roleService.UpdateAsync(id, dto);
            return Ok(ApiResponse<RoleDto>.SuccessResponse(result));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating role {Id}", id);
            return BadRequest(ApiResponse<RoleDto>.ErrorResponse(ex.Message));
        }
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult<ApiResponse<object>>> Delete(Guid id)
    {
        try
        {
            await _roleService.DeleteAsync(id);
            return Ok(ApiResponse<object>.SuccessResponse(null, "Role berhasil dihapus"));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting role {Id}", id);
            return BadRequest(ApiResponse<object>.ErrorResponse(ex.Message));
        }
    }

    [HttpGet("{roleId}/suboutputs")]
    public async Task<ActionResult<ApiResponse<List<RoleSuboutputDto>>>> GetRoleSuboutputs(Guid roleId)
    {
        try
        {
            var result = await _roleService.GetRoleSuboutputsAsync(roleId);
            return Ok(ApiResponse<List<RoleSuboutputDto>>.SuccessResponse(result));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting role suboutputs for role {RoleId}", roleId);
            return StatusCode(500, ApiResponse<List<RoleSuboutputDto>>.ErrorResponse("Internal server error"));
        }
    }

    [HttpPost("{roleId}/suboutputs")]
    public async Task<ActionResult<ApiResponse<object>>> AssignSuboutputs(Guid roleId, [FromBody] AssignSuboutputsDto dto)
    {
        try
        {
            await _roleService.AssignSuboutputsAsync(roleId, dto);
            return Ok(ApiResponse<object>.SuccessResponse(null, "Suboutputs berhasil di-assign"));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error assigning suboutputs to role {RoleId}", roleId);
            return BadRequest(ApiResponse<object>.ErrorResponse(ex.Message));
        }
    }
}
