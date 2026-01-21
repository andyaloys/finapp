using FinApp.Core.DTOs.Common;
using FinApp.Core.DTOs.Stpb;
using FinApp.Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FinApp.API.Controllers;

[Authorize]
public class StpbController : BaseApiController
{
    private readonly IStpbService _stpbService;
    private readonly ILogger<StpbController> _logger;

    public StpbController(IStpbService stpbService, ILogger<StpbController> logger)
    {
        _stpbService = stpbService;
        _logger = logger;
    }

    [HttpGet]
    public async Task<ActionResult<ApiResponse<PagedResult<StpbDto>>>> GetAll(
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10,
        [FromQuery] string? searchTerm = null)
    {
        try
        {
            var result = await _stpbService.GetAllAsync(pageNumber, pageSize, searchTerm);
            return Ok(ApiResponse<PagedResult<StpbDto>>.SuccessResponse(result));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting STPB list");
            return StatusCode(500, ApiResponse<PagedResult<StpbDto>>.ErrorResponse("Internal server error"));
        }
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ApiResponse<StpbDto>>> GetById(Guid id)
    {
        try
        {
            var result = await _stpbService.GetByIdAsync(id);
            
            if (result == null)
            {
                return NotFound(ApiResponse<StpbDto>.ErrorResponse($"STPB with ID {id} not found"));
            }

            return Ok(ApiResponse<StpbDto>.SuccessResponse(result));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting STPB {Id}", id);
            return StatusCode(500, ApiResponse<StpbDto>.ErrorResponse("Internal server error"));
        }
    }

    [HttpPost]
    public async Task<ActionResult<ApiResponse<StpbDto>>> Create([FromBody] CreateStpbDto dto)
    {
        try
        {
            var userId = GetUserId();
            var result = await _stpbService.CreateAsync(dto, userId);
            
            return CreatedAtAction(
                nameof(GetById), 
                new { id = result.Id }, 
                ApiResponse<StpbDto>.SuccessResponse(result, "STPB created successfully")
            );
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating STPB");
            return BadRequest(ApiResponse<StpbDto>.ErrorResponse(ex.Message));
        }
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<ApiResponse<StpbDto>>> Update(Guid id, [FromBody] UpdateStpbDto dto)
    {
        try
        {
            var result = await _stpbService.UpdateAsync(id, dto);
            return Ok(ApiResponse<StpbDto>.SuccessResponse(result, "STPB updated successfully"));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating STPB {Id}", id);
            
            if (ex.Message.Contains("not found"))
            {
                return NotFound(ApiResponse<StpbDto>.ErrorResponse(ex.Message));
            }
            
            return BadRequest(ApiResponse<StpbDto>.ErrorResponse(ex.Message));
        }
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult<ApiResponse<bool>>> Delete(Guid id)
    {
        try
        {
            var result = await _stpbService.DeleteAsync(id);
            return Ok(ApiResponse<bool>.SuccessResponse(result, "STPB deleted successfully"));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting STPB {Id}", id);
            
            if (ex.Message.Contains("not found"))
            {
                return NotFound(ApiResponse<bool>.ErrorResponse(ex.Message));
            }
            
            return StatusCode(500, ApiResponse<bool>.ErrorResponse("Internal server error"));
        }
    }

    [HttpGet("my")]
    public async Task<ActionResult<ApiResponse<IEnumerable<StpbDto>>>> GetMyStpbs()
    {
        try
        {
            var userId = GetUserId();
            var result = await _stpbService.GetByUserIdAsync(userId);
            return Ok(ApiResponse<IEnumerable<StpbDto>>.SuccessResponse(result));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting user STPB list");
            return StatusCode(500, ApiResponse<IEnumerable<StpbDto>>.ErrorResponse("Internal server error"));
        }
    }
}
