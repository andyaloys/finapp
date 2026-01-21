using FinApp.Core.DTOs.Common;
using FinApp.Core.DTOs.Output;
using FinApp.Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FinApp.API.Controllers;

[Authorize]
public class OutputController : BaseApiController
{
    private readonly IOutputService _outputService;

    public OutputController(IOutputService outputService)
    {
        _outputService = outputService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll(
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 1000,
        [FromQuery] string? searchTerm = null)
    {
        var result = await _outputService.GetAllAsync(pageNumber, pageSize, searchTerm);
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
        var output = await _outputService.GetByIdAsync(id);
        return Ok(new { success = true, data = output });
    }

    [HttpGet("kegiatan/{kegiatanId}")]
    public async Task<IActionResult> GetByKegiatanId(Guid kegiatanId)
    {
        var outputs = await _outputService.GetByKegiatanIdAsync(kegiatanId);
        return Ok(new { success = true, data = outputs });
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateOutputDto createDto)
    {
        var output = await _outputService.CreateAsync(createDto);
        return Ok(new { success = true, data = output });
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(Guid id, UpdateOutputDto updateDto)
    {
        var output = await _outputService.UpdateAsync(id, updateDto);
        return Ok(new { success = true, data = output });
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        await _outputService.DeleteAsync(id);
        return Ok(new { success = true, message = "Output berhasil dihapus" });
    }
}
