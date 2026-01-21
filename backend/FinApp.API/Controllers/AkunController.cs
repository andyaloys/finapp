using FinApp.Core.DTOs.Akun;
using FinApp.Core.DTOs.Common;
using FinApp.Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FinApp.API.Controllers;

[Authorize]
public class AkunController : BaseApiController
{
    private readonly IAkunService _akunService;

    public AkunController(IAkunService akunService)
    {
        _akunService = akunService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll(
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 1000,
        [FromQuery] string? searchTerm = null)
    {
        var result = await _akunService.GetAllAsync(pageNumber, pageSize, searchTerm);
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
        var akun = await _akunService.GetByIdAsync(id);
        return Ok(new { success = true, data = akun });
    }

    [HttpGet("subkomponen/{subkomponenId}")]
    public async Task<IActionResult> GetBySubkomponenId(Guid subkomponenId)
    {
        var akuns = await _akunService.GetBySubkomponenIdAsync(subkomponenId);
        return Ok(new { success = true, data = akuns });
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateAkunDto createDto)
    {
        var akun = await _akunService.CreateAsync(createDto);
        return Ok(new { success = true, data = akun });
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(Guid id, UpdateAkunDto updateDto)
    {
        var akun = await _akunService.UpdateAsync(id, updateDto);
        return Ok(new { success = true, data = akun });
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        await _akunService.DeleteAsync(id);
        return Ok(new { success = true, message = "Akun berhasil dihapus" });
    }
}
