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
            var userId = GetUserId();
            var tahunAnggaran = tahun ?? DateTime.Now.Year;
            _logger.LogInformation("Getting monitoring anggaran for tahun {Tahun} and user {UserId}", tahunAnggaran, userId);
            var result = await _monitoringService.GetMonitoringAnggaranAsync(tahunAnggaran, userId);
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
            var userId = GetUserId();
            var allAnggaran = await _monitoringService.GetMonitoringAnggaranAsync(2026, userId);
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

    [HttpGet("stpb-details")]
    public async Task<ActionResult<ApiResponse<IEnumerable<StpbDetailMonitoringDto>>>> GetStpbDetails(
        [FromQuery] string kodeProgram,
        [FromQuery] string kodeKegiatan,
        [FromQuery] string kodeOutput,
        [FromQuery] string kodeSuboutput,
        [FromQuery] string kodeKomponen,
        [FromQuery] string kodeSubkomponen,
        [FromQuery] string kodeAkun,
        [FromQuery] string noItem,
        [FromQuery] int tahun)
    {
        try
        {
            var userId = GetUserId();
            var result = await _monitoringService.GetStpbDetailsAsync(
                kodeProgram, kodeKegiatan, kodeOutput, kodeSuboutput,
                kodeKomponen, kodeSubkomponen, kodeAkun, noItem, tahun, userId);
            return Ok(ApiResponse<IEnumerable<StpbDetailMonitoringDto>>.SuccessResponse(result));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting STPB details");
            return StatusCode(500, ApiResponse<IEnumerable<StpbDetailMonitoringDto>>.ErrorResponse("Internal server error"));
        }
    }
}
