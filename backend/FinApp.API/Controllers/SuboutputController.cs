using FinApp.Core.DTOs.Common;
using FinApp.Core.DTOs.Suboutput;
using FinApp.Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FinApp.API.Controllers;

[Authorize]
public class SuboutputController : BaseApiController
{
    private readonly ISuboutputService _suboutputService;

    public SuboutputController(ISuboutputService suboutputService)
    {
        _suboutputService = suboutputService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll(
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 1000,
        [FromQuery] string? searchTerm = null)
    {
        var result = await _suboutputService.GetAllAsync(pageNumber, pageSize, searchTerm);
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
        var suboutput = await _suboutputService.GetByIdAsync(id);
        return Ok(new { success = true, data = suboutput });
    }

    [HttpGet("output/{outputId}")]
    public async Task<IActionResult> GetByOutputId(Guid outputId)
    {
        var suboutputs = await _suboutputService.GetByOutputIdAsync(outputId);
        return Ok(new { success = true, data = suboutputs });
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateSuboutputDto createDto)
    {
        var suboutput = await _suboutputService.CreateAsync(createDto);
        return Ok(new { success = true, data = suboutput });
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(Guid id, UpdateSuboutputDto updateDto)
    {
        var suboutput = await _suboutputService.UpdateAsync(id, updateDto);
        return Ok(new { success = true, data = suboutput });
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        await _suboutputService.DeleteAsync(id);
        return Ok(new { success = true, message = "Suboutput berhasil dihapus" });
    }
}
