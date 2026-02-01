using FinApp.Core.DTOs.Common;
using FinApp.Core.DTOs.Monitoring;
using FinApp.Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FinApp.API.Controllers;

[Authorize]
public class MonitoringController : BaseApiController
{
    private readonly IMonitoringService _monitoringService;
    private readonly ILogger<MonitoringController> _logger;

    public MonitoringController(IMonitoringService monitoringService, ILogger<MonitoringController> logger)
    {
        _monitoringService = monitoringService;
        _logger = logger;
    }

    [HttpGet("anggaran")]
    public async Task<ActionResult<ApiResponse<IEnumerable<MonitoringAnggaranDto>>>> GetMonitoringAnggaran([FromQuery] int? tahun = null)
    {
        try
        {
            var tahunAnggaran = tahun ?? DateTime.Now.Year;
            _logger.LogInformation("Getting monitoring anggaran for tahun {Tahun}", tahunAnggaran);
            var result = await _monitoringService.GetMonitoringAnggaranAsync(tahunAnggaran);
            var resultList = result.ToList();
            _logger.LogInformation("Found {Count} records for tahun {Tahun}", resultList.Count, tahunAnggaran);
            return Ok(ApiResponse<IEnumerable<MonitoringAnggaranDto>>.SuccessResponse(resultList));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting monitoring anggaran for tahun {Tahun}", tahun);
            return StatusCode(500, ApiResponse<IEnumerable<MonitoringAnggaranDto>>.ErrorResponse("Internal server error"));
        }
    }

    [HttpGet("test-anggaran")]
    public async Task<IActionResult> TestAnggaran()
    {
        try
        {
            var allAnggaran = await _monitoringService.GetMonitoringAnggaranAsync(2026);
            return Ok(new { 
                count = allAnggaran.Count(),
                data = allAnggaran.Take(5)
            });
        }
        catch (Exception ex)
        {
            return Ok(new { error = ex.Message, stack = ex.StackTrace });
        }
    }
}
