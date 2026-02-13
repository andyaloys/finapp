using FinApp.API.Controllers;
using FinApp.Core.DTOs.Common;
using FinApp.Core.DTOs.Penerima;
using FinApp.Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FinApp.API.Controllers;

[Authorize]
public class PenerimaController : BaseApiController
{
    private readonly IPenerimaService _penerimaService;

    public PenerimaController(IPenerimaService penerimaService)
    {
        _penerimaService = penerimaService;
    }

    [HttpGet]
    public async Task<ActionResult<ApiResponse<IEnumerable<PenerimaDto>>>> GetAll()
    {
        var penerimas = await _penerimaService.GetAllAsync();
        return Ok(ApiResponse<IEnumerable<PenerimaDto>>.SuccessResponse(penerimas));
    }

    [HttpGet("active")]
    public async Task<ActionResult<ApiResponse<IEnumerable<PenerimaDto>>>> GetAllActive()
    {
        var penerimas = await _penerimaService.GetAllActiveAsync();
        return Ok(ApiResponse<IEnumerable<PenerimaDto>>.SuccessResponse(penerimas));
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ApiResponse<PenerimaDto>>> GetById(int id)
    {
        var penerima = await _penerimaService.GetByIdAsync(id);
        if (penerima == null)
        {
            return NotFound(ApiResponse<PenerimaDto>.ErrorResponse($"Penerima dengan ID {id} tidak ditemukan"));
        }

        return Ok(ApiResponse<PenerimaDto>.SuccessResponse(penerima));
    }

    [HttpPost]
    public async Task<ActionResult<ApiResponse<PenerimaDto>>> Create([FromBody] CreatePenerimaDto dto)
    {
        var penerima = await _penerimaService.CreateAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = penerima.Id }, ApiResponse<PenerimaDto>.SuccessResponse(penerima, "Penerima berhasil ditambahkan"));
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<ApiResponse<PenerimaDto>>> Update(int id, [FromBody] UpdatePenerimaDto dto)
    {
        var penerima = await _penerimaService.UpdateAsync(id, dto);
        return Ok(ApiResponse<PenerimaDto>.SuccessResponse(penerima, "Penerima berhasil diupdate"));
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult<ApiResponse<object>>> Delete(int id)
    {
        await _penerimaService.DeleteAsync(id);
        return Ok(ApiResponse<object>.SuccessResponse(null, "Penerima berhasil dihapus"));
    }
}
