using FinApp.Core.DTOs.Common;
using FinApp.Core.DTOs.Komponen;
using FinApp.Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FinApp.API.Controllers;

[Authorize]
public class KomponenController : BaseApiController
{
    private readonly IKomponenService _komponenService;

    public KomponenController(IKomponenService komponenService)
    {
        _komponenService = komponenService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll(
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 1000,
        [FromQuery] string? searchTerm = null)
    {
        var result = await _komponenService.GetAllAsync(pageNumber, pageSize, searchTerm);
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
        var komponen = await _komponenService.GetByIdAsync(id);
        return Ok(new { success = true, data = komponen });
    }

    [HttpGet("suboutput/{suboutputId}")]
    public async Task<IActionResult> GetBySuboutputId(Guid suboutputId)
    {
        var komponens = await _komponenService.GetBySuboutputIdAsync(suboutputId);
        return Ok(new { success = true, data = komponens });
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateKomponenDto createDto)
    {
        var komponen = await _komponenService.CreateAsync(createDto);
        return Ok(new { success = true, data = komponen });
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(Guid id, UpdateKomponenDto updateDto)
    {
        var komponen = await _komponenService.UpdateAsync(id, updateDto);
        return Ok(new { success = true, data = komponen });
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        await _komponenService.DeleteAsync(id);
        return Ok(new { success = true, message = "Komponen berhasil dihapus" });
    }
}
