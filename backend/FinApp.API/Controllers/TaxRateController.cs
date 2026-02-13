using FinApp.Core.DTOs.TaxRate;
using FinApp.Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FinApp.API.Controllers;

[Authorize]
public class TaxRateController : BaseApiController
{
    private readonly ITaxRateService _taxRateService;

    public TaxRateController(ITaxRateService taxRateService)
    {
        _taxRateService = taxRateService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var result = await _taxRateService.GetAllAsync();
        return Ok(result);
    }

    [HttpGet("active")]
    public async Task<IActionResult> GetAllActive()
    {
        var result = await _taxRateService.GetAllActiveAsync();
        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var result = await _taxRateService.GetByIdAsync(id);
        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateTaxRateDto dto)
    {
        var result = await _taxRateService.CreateAsync(dto);
        return Ok(result);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateTaxRateDto dto)
    {
        var result = await _taxRateService.UpdateAsync(id, dto);
        return Ok(result);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var result = await _taxRateService.DeleteAsync(id);
        return Ok(result);
    }
}
