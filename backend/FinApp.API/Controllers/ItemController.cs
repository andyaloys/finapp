using FinApp.Core.DTOs.Common;
using FinApp.Core.DTOs.Item;
using FinApp.Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FinApp.API.Controllers;

[Authorize]
public class ItemController : BaseApiController
{
    private readonly IItemService _itemService;

    public ItemController(IItemService itemService)
    {
        _itemService = itemService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll(
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 1000,
        [FromQuery] string? searchTerm = null)
    {
        var result = await _itemService.GetAllAsync(pageNumber, pageSize, searchTerm);
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
        var item = await _itemService.GetByIdAsync(id);
        return Ok(new { success = true, data = item });
    }

    [HttpGet("akun/{akunId}")]
    public async Task<IActionResult> GetByAkunId(Guid akunId)
    {
        var items = await _itemService.GetByAkunIdAsync(akunId);
        return Ok(new { success = true, data = items });
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateItemDto createDto)
    {
        var item = await _itemService.CreateAsync(createDto);
        return Ok(new { success = true, data = item });
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(Guid id, UpdateItemDto updateDto)
    {
        var item = await _itemService.UpdateAsync(id, updateDto);
        return Ok(new { success = true, data = item });
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        await _itemService.DeleteAsync(id);
        return Ok(new { success = true, message = "Item berhasil dihapus" });
    }
}
