using FinApp.Core.DTOs.Common;
using FinApp.Core.DTOs.Subkomponen;
using FinApp.Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FinApp.API.Controllers;

[Authorize]
public class SubkomponenController : BaseApiController
{
    private readonly ISubkomponenService _subkomponenService;

    public SubkomponenController(ISubkomponenService subkomponenService)
    {
        _subkomponenService = subkomponenService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll(
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 1000,
        [FromQuery] string? searchTerm = null)
    {
        var result = await _subkomponenService.GetAllAsync(pageNumber, pageSize, searchTerm);
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
        var subkomponen = await _subkomponenService.GetByIdAsync(id);
        return Ok(new { success = true, data = subkomponen });
    }

    [HttpGet("komponen/{komponenId}")]
    public async Task<IActionResult> GetByKomponenId(Guid komponenId)
    {
        var subkomponens = await _subkomponenService.GetByKomponenIdAsync(komponenId);
        return Ok(new { success = true, data = subkomponens });
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateSubkomponenDto createDto)
    {
        var subkomponen = await _subkomponenService.CreateAsync(createDto);
        return Ok(new { success = true, data = subkomponen });
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(Guid id, UpdateSubkomponenDto updateDto)
    {
        var subkomponen = await _subkomponenService.UpdateAsync(id, updateDto);
        return Ok(new { success = true, data = subkomponen });
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        await _subkomponenService.DeleteAsync(id);
        return Ok(new { success = true, message = "Subkomponen berhasil dihapus" });
    }
}
