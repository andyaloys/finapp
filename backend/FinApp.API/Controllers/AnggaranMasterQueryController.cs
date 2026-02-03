using FinApp.Domain.Entities;
using FinApp.Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace FinApp.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class AnggaranMasterQueryController : ControllerBase
{
    private readonly AppDbContext _context;
    public AnggaranMasterQueryController(AppDbContext context)
    {
        _context = context;
    }

    private async Task<User?> GetCurrentUserWithRoleAsync()
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(userId))
            return null;

        return await _context.Users
            .Include(u => u.Role)
                .ThenInclude(r => r.RoleSuboutputs)
            .FirstOrDefaultAsync(u => u.Id == Guid.Parse(userId));
    }

    private IQueryable<AnggaranMaster> ApplyRoleFilter(IQueryable<AnggaranMaster> query, User user)
    {
        if (user.Role.IsAdmin)
            return query;

        var allowedSuboutputs = user.Role.RoleSuboutputs
            .Select(rs => rs.KodeSuboutput)
            .Where(k => k != null)
            .ToList();
        return query.Where(x => x.KdSOutput != null && allowedSuboutputs.Contains(x.KdSOutput));
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

    [HttpGet("all-suboutputs")]
    public async Task<IActionResult> GetAllSuboutputs()
    {
        var suboutputs = await _context.AnggaranMasters
            .Select(x => new { x.KdSOutput, x.NmSOutput })
            .Distinct()
            .OrderBy(x => x.KdSOutput)
            .ToListAsync();
        return Ok(new { success = true, data = suboutputs });
    }

    [HttpGet("check-pagu")]
    public async Task<IActionResult> CheckPagu(
        [FromQuery] int tahun,
        [FromQuery] int revisi,
        [FromQuery] string kdProgram,
        [FromQuery] string kdGiat,
        [FromQuery] string kdOutput,
        [FromQuery] string kdSOutput,
        [FromQuery] string kdKmpnen,
        [FromQuery] string kdSkmpnen,
        [FromQuery] string kdAkun,
        [FromQuery] string noItem)
    {
        // Get pagu from anggaran master
        var anggaran = await _context.AnggaranMasters
            .FirstOrDefaultAsync(x => 
                x.TahunAnggaran == tahun &&
                x.Revisi == revisi &&
                x.KdProgram == kdProgram &&
                x.KdGiat == kdGiat &&
                x.KdOutput == kdOutput &&
                x.KdSOutput == kdSOutput &&
                x.KdKmpnen == kdKmpnen &&
                x.KdSkmpnen == kdSkmpnen &&
                x.KdAkun == kdAkun &&
                x.NoItem == noItem);

        if (anggaran == null)
            return Ok(new { success = false, message = "Item tidak ditemukan di anggaran" });

        // Calculate realisasi from approved STPB details
        var realisasi = await _context.StpbDetails
            .Where(d => 
                d.Tahun == tahun &&
                d.Revisi == revisi &&
                d.KodeProgram == kdProgram &&
                d.KodeKegiatan == kdGiat &&
                d.KodeOutput == kdOutput &&
                d.KodeSuboutput == kdSOutput &&
                d.KodeKomponen == kdKmpnen &&
                d.KodeSubkomponen == kdSkmpnen &&
                d.KodeAkun == kdAkun &&
                d.NoItem == noItem &&
                d.Stpb.Status == StpbStatus.Approve)
            .SumAsync(d => d.JumlahHarga);

        var sisaPagu = anggaran.Pagu - realisasi;

        return Ok(new { 
            success = true, 
            data = new {
                pagu = anggaran.Pagu,
                realisasi = realisasi,
                sisaPagu = sisaPagu
            }
        });
    }

    [HttpGet("distinct-suboutputs")]
    public async Task<IActionResult> GetDistinctSuboutputs([FromQuery] int tahun, [FromQuery] int revisi, [FromQuery] string kdProgram, [FromQuery] string kdGiat, [FromQuery] string kdOutput)
    {
        var user = await GetCurrentUserWithRoleAsync();
        if (user == null)
            return Unauthorized();

        var query = _context.AnggaranMasters
            .Where(x => x.TahunAnggaran == tahun && x.Revisi == revisi && x.KdProgram == kdProgram && x.KdGiat == kdGiat && x.KdOutput == kdOutput);

        query = ApplyRoleFilter(query, user);

        var suboutputs = await query.Select(x => new { x.KdSOutput, x.NmSOutput })
            .Distinct()
            .ToListAsync();
        return Ok(new { success = true, data = suboutputs });
    }

    [HttpGet("distinct-komponens")]
    public async Task<IActionResult> GetDistinctKomponens([FromQuery] int tahun, [FromQuery] int revisi, [FromQuery] string kdProgram, [FromQuery] string kdGiat, [FromQuery] string kdOutput, [FromQuery] string kdSOutput)
    {
        var user = await GetCurrentUserWithRoleAsync();
        if (user == null)
            return Unauthorized();

        var query = _context.AnggaranMasters
            .Where(x => x.TahunAnggaran == tahun && x.Revisi == revisi && x.KdProgram == kdProgram && x.KdGiat == kdGiat && x.KdOutput == kdOutput && x.KdSOutput == kdSOutput);

        query = ApplyRoleFilter(query, user);

        var komponens = await query.Select(x => new { x.KdKmpnen, x.NmKmpnen })
            .Distinct()
            .ToListAsync();
        return Ok(new { success = true, data = komponens });
    }

    [HttpGet("distinct-subkomponens")]
    public async Task<IActionResult> GetDistinctSubkomponens([FromQuery] int tahun, [FromQuery] int revisi, [FromQuery] string kdProgram, [FromQuery] string kdGiat, [FromQuery] string kdOutput, [FromQuery] string kdSOutput, [FromQuery] string kdKmpnen)
    {
        var user = await GetCurrentUserWithRoleAsync();
        if (user == null)
            return Unauthorized();

        var query = _context.AnggaranMasters
            .Where(x => x.TahunAnggaran == tahun && x.Revisi == revisi && x.KdProgram == kdProgram && x.KdGiat == kdGiat && x.KdOutput == kdOutput && x.KdSOutput == kdSOutput && x.KdKmpnen == kdKmpnen);

        query = ApplyRoleFilter(query, user);

        var subkomponens = await query.Select(x => new { x.KdSkmpnen, x.NmSkmpnen })
            .Distinct()
            .ToListAsync();
        return Ok(new { success = true, data = subkomponens });
    }

    [HttpGet("distinct-akuns")]
    public async Task<IActionResult> GetDistinctAkuns([FromQuery] int tahun, [FromQuery] int revisi, [FromQuery] string kdProgram, [FromQuery] string kdGiat, [FromQuery] string kdOutput, [FromQuery] string kdSOutput, [FromQuery] string kdKmpnen, [FromQuery] string kdSkmpnen)
    {
        var user = await GetCurrentUserWithRoleAsync();
        if (user == null)
            return Unauthorized();

        var query = _context.AnggaranMasters
            .Where(x => x.TahunAnggaran == tahun && x.Revisi == revisi && x.KdProgram == kdProgram && x.KdGiat == kdGiat && x.KdOutput == kdOutput && x.KdSOutput == kdSOutput && x.KdKmpnen == kdKmpnen && x.KdSkmpnen == kdSkmpnen);

        query = ApplyRoleFilter(query, user);

        var akuns = await query.Select(x => new { x.KdAkun, x.NmAkun })
            .Distinct()
            .ToListAsync();
        return Ok(new { success = true, data = akuns });
    }

    [HttpGet("distinct-items")]
    public async Task<IActionResult> GetDistinctItems([FromQuery] int tahun, [FromQuery] int revisi, [FromQuery] string kdProgram, [FromQuery] string kdGiat, [FromQuery] string kdOutput, [FromQuery] string kdSOutput, [FromQuery] string kdKmpnen, [FromQuery] string kdSkmpnen, [FromQuery] string kdAkun)
    {
        var user = await GetCurrentUserWithRoleAsync();
        if (user == null)
            return Unauthorized();

        var query = _context.AnggaranMasters
            .Where(x => x.TahunAnggaran == tahun && x.Revisi == revisi && x.KdProgram == kdProgram && x.KdGiat == kdGiat && x.KdOutput == kdOutput && x.KdSOutput == kdSOutput && x.KdKmpnen == kdKmpnen && x.KdSkmpnen == kdSkmpnen && x.KdAkun == kdAkun);

        query = ApplyRoleFilter(query, user);

        var items = await query.Select(x => new { x.NoItem, x.NmItem, x.VolKeg, x.SatKeg, x.HargaSat, x.Pagu, x.Netto })
            .Distinct()
            .ToListAsync();
        return Ok(new { success = true, data = items });
    }

    [HttpGet("distinct-kegiatans")]
    public async Task<IActionResult> GetDistinctKegiatans([FromQuery] int tahun, [FromQuery] int revisi, [FromQuery] string kdProgram)
    {
        var user = await GetCurrentUserWithRoleAsync();
        if (user == null)
            return Unauthorized();

        var query = _context.AnggaranMasters
            .Where(x => x.TahunAnggaran == tahun && x.Revisi == revisi && x.KdProgram == kdProgram);

        query = ApplyRoleFilter(query, user);

        var kegiatans = await query.Select(x => new { x.KdGiat, x.NmGiat })
            .Distinct()
            .ToListAsync();
        return Ok(new { success = true, data = kegiatans });
    }

    [HttpGet("distinct-outputs")]
    public async Task<IActionResult> GetDistinctOutputs([FromQuery] int tahun, [FromQuery] int revisi, [FromQuery] string kdProgram, [FromQuery] string kdGiat)
    {
        var user = await GetCurrentUserWithRoleAsync();
        if (user == null)
            return Unauthorized();

        var query = _context.AnggaranMasters
            .Where(x => x.TahunAnggaran == tahun && x.Revisi == revisi && x.KdProgram == kdProgram && x.KdGiat == kdGiat);

        query = ApplyRoleFilter(query, user);

        var outputs = await query.Select(x => new { x.KdOutput, x.NmOutput })
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
        var user = await GetCurrentUserWithRoleAsync();
        if (user == null)
            return Unauthorized();

        var query = _context.AnggaranMasters
            .Where(x => x.TahunAnggaran == tahun && x.Revisi == revisi);

        query = ApplyRoleFilter(query, user);

        var programs = await query.Select(x => new { x.KdProgram, x.NmProgram })
            .Distinct()
            .ToListAsync();
        return Ok(new { success = true, data = programs });
    }

    // Tambahkan endpoint serupa untuk Kegiatan, Output, dst jika perlu cascading

    [HttpGet("summary")]
    public async Task<IActionResult> GetSummary([FromQuery] int? tahun)
    {
        var query = _context.AnggaranMasters.AsQueryable();
        
        // Filter by tahun if provided
        if (tahun.HasValue)
        {
            query = query.Where(x => x.TahunAnggaran == tahun.Value);
        }
        
        var summary = await query
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
        var user = await GetCurrentUserWithRoleAsync();

        if (user == null)
            return Unauthorized(new { success = false, message = "User not found" });

        var query = _context.AnggaranMasters
            .Where(x => x.TahunAnggaran == tahun && x.Revisi == revisi);

        // Filter by role suboutputs if not admin
        if (!user.Role.IsAdmin)
        {
            var allowedSuboutputs = user.Role.RoleSuboutputs.Select(rs => rs.KodeSuboutput).ToList();
            query = query.Where(x => allowedSuboutputs.Contains(x.KdSOutput));
        }

        var details = await query
            .Select(x => new {
                kdProgram = x.KdProgram,
                kdGiat = x.KdGiat,
                kdOutput = x.KdOutput,
                kdSOutput = x.KdSOutput,
                kdKmpnen = x.KdKmpnen,
                kdSkmpnen = x.KdSkmpnen,
                kdAkun = x.KdAkun,
                noItem = x.NoItem,
                nmItem = x.NmItem,
                hargaSat = x.HargaSat,
                pagu = x.Pagu
            })
            .ToListAsync();
        return Ok(new { success = true, data = details });
    }
}
