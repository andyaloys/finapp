using FinApp.Core.DTOs.Common;
using FinApp.Core.DTOs.Kegiatan;
using FinApp.Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FinApp.API.Controllers;

[Authorize]
public class KegiatanController : BaseApiController
{
    private readonly IKegiatanService _kegiatanService;

    public KegiatanController(IKegiatanService kegiatanService)
    {
        _kegiatanService = kegiatanService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll(
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 1000,
        [FromQuery] string? searchTerm = null)
    {
        var result = await _kegiatanService.GetAllAsync(pageNumber, pageSize, searchTerm);
        return Ok(new { 
            success = true, 
            data = new { 
                items = result.Items,
                pageNumber = result.PageNumber,
                pageSize = result.PageSize,
                totalCount = result.TotalCount
            }
        });
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var kegiatan = await _kegiatanService.GetByIdAsync(id);
        return Ok(new { success = true, data = kegiatan });
    }

    [HttpGet("program/{programId}")]
    public async Task<IActionResult> GetByProgramId(Guid programId)
    {
        var kegiatans = await _kegiatanService.GetByProgramIdAsync(programId);
        return Ok(new { success = true, data = kegiatans });
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateKegiatanDto createDto)
    {
        var kegiatan = await _kegiatanService.CreateAsync(createDto);
        return Ok(new { success = true, data = kegiatan });
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(Guid id, UpdateKegiatanDto updateDto)
    {
        var kegiatan = await _kegiatanService.UpdateAsync(id, updateDto);
        return Ok(new { success = true, data = kegiatan });
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        await _kegiatanService.DeleteAsync(id);
        return Ok(new { success = true, message = "Kegiatan berhasil dihapus" });
        return NoContent();
    }
}
