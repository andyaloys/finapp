using FinApp.Domain.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FinApp.API.Controllers;

[Authorize]
public class DebugController : BaseApiController
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<DebugController> _logger;

    public DebugController(IUnitOfWork unitOfWork, ILogger<DebugController> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    [HttpGet("check-monitoring")]
    public async Task<IActionResult> CheckMonitoring([FromQuery] int tahun = 2026)
    {
        var anggaran = await _unitOfWork.AnggaranMasters.GetAllAsync();
        var anggaranForYear = anggaran.Where(a => a.TahunAnggaran == tahun).ToList();

        var stpbs = await _unitOfWork.Stpbs.GetAllAsync();
        var stpbsForYear = stpbs.Where(s => s.Tahun == tahun).ToList();

        var stpbDetails = await _unitOfWork.StpbDetails.GetAllAsync();
        var stpbIdsForYear = stpbsForYear.Select(s => s.Id).ToHashSet();
        var detailsForYear = stpbDetails.Where(sd => stpbIdsForYear.Contains(sd.StpbId)).ToList();

        // Sample data
        var sampleAnggaran = anggaranForYear.Take(3).Select(a => new
        {
            KdProgram = a.KdProgram,
            KdGiat = a.KdGiat,
            KdOutput = a.KdOutput,
            KdSOutput = a.KdSOutput,
            KdKmpnen = a.KdKmpnen,
            KdSkmpnen = a.KdSkmpnen,
            KdAkun = a.KdAkun,
            NoItem = a.NoItem,
            NmItem = a.NmItem,
            Netto = a.Netto
        }).ToList();

        var sampleStpb = stpbsForYear.Take(3).Select(s => new
        {
            NomorSTPB = s.NomorSTPB,
            Tahun = s.Tahun,
            Status = s.Status.ToString()
        }).ToList();

        var sampleDetails = detailsForYear.Take(5).Select(sd => new
        {
            KodeProgram = sd.KodeProgram,
            KodeKegiatan = sd.KodeKegiatan,
            KodeOutput = sd.KodeOutput,
            KodeSuboutput = sd.KodeSuboutput,
            KodeKomponen = sd.KodeKomponen,
            KodeSubkomponen = sd.KodeSubkomponen,
            KodeAkun = sd.KodeAkun,
            NoItem = sd.NoItem,
            NilaiBersih = sd.NilaiBersih,
            Keterangan = sd.Keterangan
        }).ToList();

        return Ok(new
        {
            summary = new
            {
                totalAnggaran = anggaranForYear.Count,
                totalStpb = stpbsForYear.Count,
                totalStpbDetails = detailsForYear.Count
            },
            sampleAnggaran,
            sampleStpb,
            sampleDetails
        });
    }
}
