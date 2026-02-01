using FinApp.API.Controllers;
using FinApp.Core.DTOs.Common;
using FinApp.Core.DTOs.PpkBendahara;
using FinApp.Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FinApp.API.Controllers;

[Authorize]
public class PpkBendaharaController : BaseApiController
{
    private readonly IPpkBendaharaService _ppkBendaharaService;
    private readonly ILogger<PpkBendaharaController> _logger;

    public PpkBendaharaController(
        IPpkBendaharaService ppkBendaharaService,
        ILogger<PpkBendaharaController> logger)
    {
        _ppkBendaharaService = ppkBendaharaService;
        _logger = logger;
    }

    [HttpGet]
    public async Task<ActionResult<ApiResponse<IEnumerable<PpkBendaharaDto>>>> GetAll()
    {
        try
        {
            var result = await _ppkBendaharaService.GetAllAsync();
            return Ok(ApiResponse<IEnumerable<PpkBendaharaDto>>.SuccessResponse(result));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting all PPK/Bendahara");
            return StatusCode(500, ApiResponse<IEnumerable<PpkBendaharaDto>>.ErrorResponse("Internal server error"));
        }
    }

    [HttpGet("active")]
    public async Task<ActionResult<ApiResponse<IEnumerable<PpkBendaharaDto>>>> GetActive()
    {
        try
        {
            var result = await _ppkBendaharaService.GetActiveAsync();
            return Ok(ApiResponse<IEnumerable<PpkBendaharaDto>>.SuccessResponse(result));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting active PPK/Bendahara");
            return StatusCode(500, ApiResponse<IEnumerable<PpkBendaharaDto>>.ErrorResponse("Internal server error"));
        }
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ApiResponse<PpkBendaharaDto>>> GetById(Guid id)
    {
        try
        {
            var result = await _ppkBendaharaService.GetByIdAsync(id);
            return Ok(ApiResponse<PpkBendaharaDto>.SuccessResponse(result));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting PPK/Bendahara {Id}", id);
            return NotFound(ApiResponse<PpkBendaharaDto>.ErrorResponse(ex.Message));
        }
    }

    [HttpPost]
    public async Task<ActionResult<ApiResponse<PpkBendaharaDto>>> Create([FromBody] CreatePpkBendaharaDto dto)
    {
        try
        {
            var result = await _ppkBendaharaService.CreateAsync(dto);
            return CreatedAtAction(
                nameof(GetById),
                new { id = result.Id },
                ApiResponse<PpkBendaharaDto>.SuccessResponse(result, "PPK/Bendahara created successfully")
            );
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating PPK/Bendahara");
            return BadRequest(ApiResponse<PpkBendaharaDto>.ErrorResponse(ex.Message));
        }
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<ApiResponse<PpkBendaharaDto>>> Update(Guid id, [FromBody] UpdatePpkBendaharaDto dto)
    {
        try
        {
            var result = await _ppkBendaharaService.UpdateAsync(id, dto);
            return Ok(ApiResponse<PpkBendaharaDto>.SuccessResponse(result, "PPK/Bendahara updated successfully"));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating PPK/Bendahara {Id}", id);
            return BadRequest(ApiResponse<PpkBendaharaDto>.ErrorResponse(ex.Message));
        }
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult<ApiResponse<bool>>> Delete(Guid id)
    {
        try
        {
            var result = await _ppkBendaharaService.DeleteAsync(id);
            return Ok(ApiResponse<bool>.SuccessResponse(result, "PPK/Bendahara deleted successfully"));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting PPK/Bendahara {Id}", id);
            return BadRequest(ApiResponse<bool>.ErrorResponse(ex.Message));
        }
    }
}
