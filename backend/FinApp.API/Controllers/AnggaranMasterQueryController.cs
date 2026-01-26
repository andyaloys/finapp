using FinApp.Domain.Entities;
using FinApp.Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FinApp.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AnggaranMasterQueryController : ControllerBase
{
    private readonly AppDbContext _context;
    public AnggaranMasterQueryController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet("distinct-tahun")]
    public async Task<IActionResult> GetDistinctTahun()
    {
        var tahunList = await _context.AnggaranMasters
            .Select(a => a.TahunAnggaran)
            .Distinct()
            .OrderByDescending(t => t)
            .ToListAsync();
        return Ok(new { success = true, data = tahunList });
    }

    [HttpGet("distinct-revisi")]
    public async Task<IActionResult> GetDistinctRevisi([FromQuery] int tahunAnggaran)
    {
        var revisiList = await _context.AnggaranMasters
            .Where(a => a.TahunAnggaran == tahunAnggaran)
            .Select(a => a.Revisi)
            .Distinct()
            .OrderByDescending(r => r)
            .ToListAsync();
        return Ok(new { success = true, data = revisiList });
    }

    [HttpGet("distinct-suboutputs")]
    public async Task<IActionResult> GetDistinctSuboutputs([FromQuery] int tahun, [FromQuery] int revisi, [FromQuery] string kdProgram, [FromQuery] string kdGiat, [FromQuery] string kdOutput)
    {
        var suboutputs = await _context.AnggaranMasters
            .Where(x => x.TahunAnggaran == tahun && x.Revisi == revisi && x.KdProgram == kdProgram && x.KdGiat == kdGiat && x.KdOutput == kdOutput)
            .Select(x => new { x.KdSOutput, x.NmSOutput })
            .Distinct()
            .ToListAsync();
        return Ok(new { success = true, data = suboutputs });
    }

    [HttpGet("distinct-komponens")]
    public async Task<IActionResult> GetDistinctKomponens([FromQuery] int tahun, [FromQuery] int revisi, [FromQuery] string kdProgram, [FromQuery] string kdGiat, [FromQuery] string kdOutput, [FromQuery] string kdSOutput)
    {
        var komponens = await _context.AnggaranMasters
            .Where(x => x.TahunAnggaran == tahun && x.Revisi == revisi && x.KdProgram == kdProgram && x.KdGiat == kdGiat && x.KdOutput == kdOutput && x.KdSOutput == kdSOutput)
            .Select(x => new { x.KdKmpnen, x.NmKmpnen })
            .Distinct()
            .ToListAsync();
        return Ok(new { success = true, data = komponens });
    }

    [HttpGet("distinct-subkomponens")]
    public async Task<IActionResult> GetDistinctSubkomponens([FromQuery] int tahun, [FromQuery] int revisi, [FromQuery] string kdProgram, [FromQuery] string kdGiat, [FromQuery] string kdOutput, [FromQuery] string kdSOutput, [FromQuery] string kdKmpnen)
    {
        var subkomponens = await _context.AnggaranMasters
            .Where(x => x.TahunAnggaran == tahun && x.Revisi == revisi && x.KdProgram == kdProgram && x.KdGiat == kdGiat && x.KdOutput == kdOutput && x.KdSOutput == kdSOutput && x.KdKmpnen == kdKmpnen)
            .Select(x => new { x.KdSkmpnen, x.NmSkmpnen })
            .Distinct()
            .ToListAsync();
        return Ok(new { success = true, data = subkomponens });
    }

    [HttpGet("distinct-akuns")]
    public async Task<IActionResult> GetDistinctAkuns([FromQuery] int tahun, [FromQuery] int revisi, [FromQuery] string kdProgram, [FromQuery] string kdGiat, [FromQuery] string kdOutput, [FromQuery] string kdSOutput, [FromQuery] string kdKmpnen, [FromQuery] string kdSkmpnen)
    {
        var akuns = await _context.AnggaranMasters
            .Where(x => x.TahunAnggaran == tahun && x.Revisi == revisi && x.KdProgram == kdProgram && x.KdGiat == kdGiat && x.KdOutput == kdOutput && x.KdSOutput == kdSOutput && x.KdKmpnen == kdKmpnen && x.KdSkmpnen == kdSkmpnen)
            .Select(x => new { x.KdAkun, x.NmAkun })
            .Distinct()
            .ToListAsync();
        return Ok(new { success = true, data = akuns });
    }

    [HttpGet("distinct-items")]
    public async Task<IActionResult> GetDistinctItems([FromQuery] int tahun, [FromQuery] int revisi, [FromQuery] string kdProgram, [FromQuery] string kdGiat, [FromQuery] string kdOutput, [FromQuery] string kdSOutput, [FromQuery] string kdKmpnen, [FromQuery] string kdSkmpnen, [FromQuery] string kdAkun)
    {
        var items = await _context.AnggaranMasters
            .Where(x => x.TahunAnggaran == tahun && x.Revisi == revisi && x.KdProgram == kdProgram && x.KdGiat == kdGiat && x.KdOutput == kdOutput && x.KdSOutput == kdSOutput && x.KdKmpnen == kdKmpnen && x.KdSkmpnen == kdSkmpnen && x.KdAkun == kdAkun)
            .Select(x => new { x.NoItem, x.NmItem, x.VolKeg, x.SatKeg, x.HargaSat, x.Pagu, x.Netto })
            .Distinct()
            .ToListAsync();
        return Ok(new { success = true, data = items });
    }

    [HttpGet("distinct-kegiatans")]
    public async Task<IActionResult> GetDistinctKegiatans([FromQuery] int tahun, [FromQuery] int revisi, [FromQuery] string kdProgram)
    {
        var kegiatans = await _context.AnggaranMasters
            .Where(x => x.TahunAnggaran == tahun && x.Revisi == revisi && x.KdProgram == kdProgram)
            .Select(x => new { x.KdGiat, x.NmGiat })
            .Distinct()
            .ToListAsync();
        return Ok(new { success = true, data = kegiatans });
    }

    [HttpGet("distinct-outputs")]
    public async Task<IActionResult> GetDistinctOutputs([FromQuery] int tahun, [FromQuery] int revisi, [FromQuery] string kdProgram, [FromQuery] string kdGiat)
    {
        var outputs = await _context.AnggaranMasters
            .Where(x => x.TahunAnggaran == tahun && x.Revisi == revisi && x.KdProgram == kdProgram && x.KdGiat == kdGiat)
            .Select(x => new { x.KdOutput, x.NmOutput })
            .Distinct()
            .ToListAsync();
        return Ok(new { success = true, data = outputs });
    }

    [HttpGet("items")]
    public async Task<IActionResult> GetItems([FromQuery] int tahun, [FromQuery] int revisi)
    {
        var items = await _context.AnggaranMasters
            .Where(x => x.TahunAnggaran == tahun && x.Revisi == revisi)
            .ToListAsync();
        return Ok(new { success = true, data = items });
    }

    [HttpGet("distinct-programs")]
    public async Task<IActionResult> GetDistinctPrograms([FromQuery] int tahun, [FromQuery] int revisi)
    {
        var programs = await _context.AnggaranMasters
            .Where(x => x.TahunAnggaran == tahun && x.Revisi == revisi)
            .Select(x => new { x.KdProgram, x.NmProgram })
            .Distinct()
            .ToListAsync();
        return Ok(new { success = true, data = programs });
    }

    // Tambahkan endpoint serupa untuk Kegiatan, Output, dst jika perlu cascading

    [HttpGet("summary")]
    public async Task<IActionResult> GetSummary()
    {
        var summary = await _context.AnggaranMasters
            .GroupBy(x => new { x.TahunAnggaran, x.Revisi })
            .Select(g => new {
                tahunAnggaran = g.Key.TahunAnggaran,
                revisi = g.Key.Revisi,
                jumlah = g.Count()
            })
            .OrderByDescending(x => x.tahunAnggaran)
            .ThenByDescending(x => x.revisi)
            .ToListAsync();
        return Ok(new { success = true, data = summary });
    }

    [HttpGet("detail")]
    public async Task<IActionResult> GetDetail([FromQuery] int tahun, [FromQuery] int revisi)
    {
        var details = await _context.AnggaranMasters
            .Where(x => x.TahunAnggaran == tahun && x.Revisi == revisi)
            .Select(x => new {
                kdProgram = x.KdProgram,
                kdGiat = x.KdGiat,
                kdOutput = x.KdOutput,
                kdSOutput = x.KdSOutput,
                kdKmpnen = x.KdKmpnen,
                kdSkmpnen = x.KdSkmpnen,
                kdAkun = x.KdAkun,
                noItem = x.NoItem,
                pagu = x.Pagu
            })
            .ToListAsync();
        return Ok(new { success = true, data = details });
    }
}
