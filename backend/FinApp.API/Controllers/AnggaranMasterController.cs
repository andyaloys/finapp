using System.Globalization;
using System.Text;
using CsvHelper;
using CsvHelper.Configuration;
using FinApp.Domain.Entities;
using FinApp.Infrastructure.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FinApp.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class AnggaranMasterController : ControllerBase
{
    private readonly AppDbContext _context;
    private readonly ILogger<AnggaranMasterController> _logger;

    public AnggaranMasterController(AppDbContext context, ILogger<AnggaranMasterController> logger)
    {
        _context = context;
        _logger = logger;
    }

    [HttpPost("upload")]
    public async Task<IActionResult> Upload([FromForm] IFormFile file, [FromForm] int tahunAnggaran)
    {
        try
        {
            _logger.LogInformation("Upload request received - File: {FileName}, Tahun: {Tahun}", file?.FileName, tahunAnggaran);

            if (file == null || file.Length == 0)
            {
                _logger.LogWarning("File tidak ditemukan atau kosong");
                return BadRequest(new { message = "File tidak ditemukan atau kosong" });
            }

            if (tahunAnggaran < 2000 || tahunAnggaran > 2100)
            {
                _logger.LogWarning("Tahun anggaran tidak valid: {Tahun}", tahunAnggaran);
                return BadRequest(new { message = "Tahun anggaran tidak valid" });
            }

            // Cari revisi terakhir
            int lastRevisi = await _context.AnggaranMasters
                .Where(x => x.TahunAnggaran == tahunAnggaran)
                .Select(x => (int?)x.Revisi)
                .MaxAsync() ?? -1;
            int newRevisi = lastRevisi + 1;

            _logger.LogInformation("Revisi baru: {Revisi}", newRevisi);

            // Hapus data lama untuk tahun+revisi ini (replace all)
            var toDelete = _context.AnggaranMasters.Where(x => x.TahunAnggaran == tahunAnggaran && x.Revisi == newRevisi);
            _context.AnggaranMasters.RemoveRange(toDelete);
            await _context.SaveChangesAsync();

            var records = new List<AnggaranMaster>();
            using (var stream = file.OpenReadStream())
            using (var reader = new StreamReader(stream, Encoding.UTF8))
            using (var csv = new CsvReader(reader, new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                Delimiter = ";",
                HeaderValidated = null,
                MissingFieldFound = null,
                TrimOptions = TrimOptions.Trim
            }))
            {
                await csv.ReadAsync();
                csv.ReadHeader();
                
                int rowCount = 0;
                while (await csv.ReadAsync())
                {
                    rowCount++;
                    var record = new AnggaranMaster
                    {
                        TahunAnggaran = tahunAnggaran,
                        Revisi = newRevisi,
                        KdDept = csv.GetField("kddept"),
                        NmDept = csv.GetField("nmdept"),
                        KdUnit = csv.GetField("kdunit"),
                        NmUnit = csv.GetField("nmunit"),
                        KdDekon = csv.GetField("kddekon"),
                        NmDekon = csv.GetField("nmdekon"),
                        KdSatker = csv.GetField("kdsatker"),
                        NmSatker = csv.GetField("nmsatker"),
                        KdFungsi = csv.GetField("kdfungsi"),
                        NmFungsi = csv.GetField("nmfungsi"),
                        KdSFung = csv.GetField("kdsfung"),
                        NmSFung = csv.GetField("nmsfung"),
                        KdProgram = csv.GetField("kdprogram"),
                        NmProgram = csv.GetField("nmprogram"),
                        KdGiat = csv.GetField("kdgiat"),
                        NmGiat = csv.GetField("nmgiat"),
                        KdOutput = csv.GetField("kdoutput"),
                        NmOutput = csv.GetField("nmoutput"),
                        KdSOutput = csv.GetField("kdsoutput"),
                        NmSOutput = csv.GetField("nmsoutput"),
                        KdKmpnen = csv.GetField("kdkmpnen"),
                        NmKmpnen = csv.GetField("nmkmpnen"),
                        KdSkmpnen = csv.GetField("kdskmpnen"),
                        NmSkmpnen = csv.GetField("nmskmpnen"),
                        KdAkun = csv.GetField("kdakun"),
                        NmAkun = csv.GetField("nmakun"),
                        Bkpk = csv.GetField("bkpk"),
                        KdSDana = csv.GetField("kdsdana"),
                        NmSDana = csv.GetField("nmsdana"),
                        KdBeban = csv.GetField("kdbeban"),
                        Register = csv.GetField("register"),
                        NoItem = csv.GetField("noitem"),
                        NmItem = csv.GetField("nmitem"),
                        VolKeg = csv.GetField("volkeg"),
                        SatKeg = csv.GetField("satkeg"),
                        HargaSat = TryParseDecimal(csv.GetField("hargasat")),
                        KomponenPendukung = csv.GetField("komponen_pendukung"),
                        Pagu = TryParseDecimal(csv.GetField("pagu")),
                        HasilReviuKonsolidasiBaru = TryParseDecimal(csv.GetField("hasil_reviu_konsolidasi_baru")),
                        Netto = TryParseDecimal(csv.GetField("Netto"))
                    };
                    records.Add(record);
                }
                
                _logger.LogInformation("Berhasil parse {Count} baris dari CSV", rowCount);
            }
            
            if (records.Count == 0)
            {
                _logger.LogWarning("Tidak ada data yang diparse dari file CSV");
                return BadRequest(new { message = "File CSV kosong atau format tidak sesuai" });
            }

            _context.AnggaranMasters.AddRange(records);
            await _context.SaveChangesAsync();
            
            _logger.LogInformation("Upload berhasil - {Count} records disimpan", records.Count);
            return Ok(new { success = true, count = records.Count, revisi = newRevisi });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error saat upload anggaran - Tahun: {Tahun}", tahunAnggaran);
            return StatusCode(500, new { message = $"Error: {ex.Message}" });
        }
    }

    private decimal? TryParseDecimal(string? value)
    {
        if (decimal.TryParse(value?.Replace(".", "").Replace(",", "."), NumberStyles.Any, CultureInfo.InvariantCulture, out var result))
            return result;
        return null;
    }
}
