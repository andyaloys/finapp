using FinApp.Core.DTOs.Common;
using FinApp.Core.DTOs.Stpb;
using FinApp.Core.Interfaces;
using FinApp.Core.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FinApp.API.Controllers;

[Authorize]
public class StpbController : BaseApiController
{
    private readonly IStpbService _stpbService;
    private readonly ILogger<StpbController> _logger;
    private readonly IStpbPdfService _pdfService;

    public StpbController(IStpbService stpbService, ILogger<StpbController> logger, IStpbPdfService pdfService)
    {
        _stpbService = stpbService;
        _logger = logger;
        _pdfService = pdfService;
    }

    [HttpGet]
    public async Task<ActionResult<ApiResponse<PagedResult<StpbDto>>>> GetAll(
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10,
        [FromQuery] string? searchTerm = null)
    {
        try
        {
            var userId = GetUserId();
            var result = await _stpbService.GetAllAsync(pageNumber, pageSize, searchTerm, userId);
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

    // Workflow endpoints
    [HttpPost("{id}/kirim")]
    public async Task<ActionResult<ApiResponse<StpbDto>>> Kirim(Guid id)
    {
        try
        {
            var userId = GetUserId();
            var result = await _stpbService.KirimAsync(id, userId);
            return Ok(ApiResponse<StpbDto>.SuccessResponse(result, "STPB berhasil dikirim"));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error sending STPB {Id}", id);
            return BadRequest(ApiResponse<StpbDto>.ErrorResponse(ex.Message));
        }
    }

    [HttpPost("{id}/approve")]
    public async Task<ActionResult<ApiResponse<StpbDto>>> Approve(Guid id)
    {
        try
        {
            var userId = GetUserId();
            var result = await _stpbService.ApproveAsync(id, userId);
            return Ok(ApiResponse<StpbDto>.SuccessResponse(result, "STPB berhasil di-approve"));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error approving STPB {Id}", id);
            return BadRequest(ApiResponse<StpbDto>.ErrorResponse(ex.Message));
        }
    }

    [HttpPost("{id}/kembalikan")]
    public async Task<ActionResult<ApiResponse<StpbDto>>> Kembalikan(Guid id, [FromBody] KembalikanRequest request)
    {
        try
        {
            var userId = GetUserId();
            var result = await _stpbService.KembalikanAsync(id, userId, request.Alasan);
            return Ok(ApiResponse<StpbDto>.SuccessResponse(result, "STPB berhasil dikembalikan"));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error returning STPB {Id}", id);
            return BadRequest(ApiResponse<StpbDto>.ErrorResponse(ex.Message));
        }
    }

    // Detail management endpoints
    [HttpPost("{stpbId}/details")]
    public async Task<ActionResult<ApiResponse<StpbDetailDto>>> AddDetail(Guid stpbId, [FromBody] CreateStpbDetailDto dto)
    {
        try
        {
            var userId = GetUserId();
            var result = await _stpbService.AddDetailAsync(stpbId, dto, userId);
            return Ok(ApiResponse<StpbDetailDto>.SuccessResponse(result, "Detail berhasil ditambahkan"));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error adding detail to STPB {StpbId}", stpbId);
            return BadRequest(ApiResponse<StpbDetailDto>.ErrorResponse(ex.Message));
        }
    }

    [HttpPut("{stpbId}/details/{detailId}")]
    public async Task<ActionResult<ApiResponse<StpbDetailDto>>> UpdateDetail(
        Guid stpbId, 
        Guid detailId, 
        [FromBody] CreateStpbDetailDto dto)
    {
        try
        {
            var userId = GetUserId();
            var result = await _stpbService.UpdateDetailAsync(stpbId, detailId, dto, userId);
            return Ok(ApiResponse<StpbDetailDto>.SuccessResponse(result, "Detail berhasil diupdate"));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating detail {DetailId} in STPB {StpbId}", detailId, stpbId);
            return BadRequest(ApiResponse<StpbDetailDto>.ErrorResponse(ex.Message));
        }
    }

    [HttpDelete("{stpbId}/details/{detailId}")]
    public async Task<ActionResult<ApiResponse<bool>>> DeleteDetail(Guid stpbId, Guid detailId)
    {
        try
        {
            var userId = GetUserId();
            var result = await _stpbService.DeleteDetailAsync(stpbId, detailId, userId);
            return Ok(ApiResponse<bool>.SuccessResponse(result, "Detail berhasil dihapus"));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting detail {DetailId} from STPB {StpbId}", detailId, stpbId);
            return BadRequest(ApiResponse<bool>.ErrorResponse(ex.Message));
        }
    }

    [HttpGet("{id}/pdf")]
    public async Task<IActionResult> PrintPdf(string id)
    {
        try
        {
            var userId = GetUserId();
            var pdfBytes = await _pdfService.GenerateStpbPdfAsync(Guid.Parse(id));
            
            return File(pdfBytes, "application/pdf", $"STPB-{id}.pdf");
        }
        catch (FileNotFoundException ex)
        {
            return NotFound(ApiResponse<bool>.ErrorResponse(ex.Message));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error generating PDF for STPB {StpbId}", id);
            return StatusCode(500, ApiResponse<bool>.ErrorResponse("Error generating PDF"));
        }
    }
}

// Request model for Kembalikan endpoint
public class KembalikanRequest
{
    public string Alasan { get; set; } = string.Empty;
}
