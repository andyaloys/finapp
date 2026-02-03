using FinApp.Core.DTOs.Monitoring;
using FinApp.Core.Interfaces;
using FinApp.Domain.Interfaces;

namespace FinApp.Core.Services;

public class MonitoringService : IMonitoringService
{
    private readonly IUnitOfWork _unitOfWork;

    public MonitoringService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<IEnumerable<MonitoringAnggaranDto>> GetMonitoringAnggaranAsync(int tahun)
    {
        // Get all anggaran data
        var anggaranList = await _unitOfWork.AnggaranMasters.GetAllAsync();
        
        if (!anggaranList.Any())
        {
            return Enumerable.Empty<MonitoringAnggaranDto>();
        }

        // Check if there's data for requested year
        var anggaranForYear = anggaranList.Where(a => a.TahunAnggaran == tahun).ToList();
        
        // If no data for requested year, return empty
        if (!anggaranForYear.Any())
        {
            return Enumerable.Empty<MonitoringAnggaranDto>();
        }

        // Get latest revision for the year
        var revisiTerakhir = anggaranForYear.Max(a => a.Revisi);

        // Get anggaran data for latest revision
        var anggaranData = anggaranForYear
            .Where(a => a.Revisi == revisiTerakhir)
            .ToList();

        // Get realisasi from approved STPB details
        var allStpbDetails = await _unitOfWork.StpbDetails.GetAllAsync();
        var allStpbs = await _unitOfWork.Stpbs.GetAllAsync();
        
        var approvedStpbIds = allStpbs
            .Where(s => s.Status == Domain.Entities.StpbStatus.Approve)
            .Select(s => s.Id)
            .ToHashSet();

        var realisasiData = allStpbDetails
            .Where(sd => approvedStpbIds.Contains(sd.StpbId))
            .GroupBy(sd => new { 
                sd.KodeProgram, 
                sd.KodeKegiatan, 
                sd.KodeOutput, 
                sd.KodeSuboutput,
                sd.KodeKomponen,
                sd.KodeSubkomponen,
                sd.KodeAkun,
                sd.NoItem
            })
            .Select(g => new {
                g.Key.KodeProgram,
                g.Key.KodeKegiatan,
                g.Key.KodeOutput,
                g.Key.KodeSuboutput,
                g.Key.KodeKomponen,
                g.Key.KodeSubkomponen,
                g.Key.KodeAkun,
                g.Key.NoItem,
                TotalRealisasi = g.Sum(sd => sd.NilaiBersih)
            })
            .ToList();

        var result = anggaranData.Select(anggaran =>
        {
            var realisasi = realisasiData
                .FirstOrDefault(r => 
                    r.KodeProgram == anggaran.KdProgram &&
                    r.KodeKegiatan == anggaran.KdGiat &&
                    r.KodeOutput == anggaran.KdOutput &&
                    r.KodeSuboutput == anggaran.KdSOutput &&
                    r.KodeKomponen == anggaran.KdKmpnen &&
                    r.KodeSubkomponen == anggaran.KdSkmpnen &&
                    r.KodeAkun == anggaran.KdAkun &&
                    r.NoItem == anggaran.NoItem
                )?.TotalRealisasi ?? 0;

            var pagu = anggaran.Netto ?? 0; // Use Netto instead of Pagu
            var sisa = pagu - realisasi;
            var persen = pagu > 0 ? (realisasi / pagu) * 100 : 0;

            return new MonitoringAnggaranDto
            {
                KodeProgram = anggaran.KdProgram ?? "",
                NamaProgram = anggaran.NmProgram ?? "",
                KodeKegiatan = anggaran.KdGiat ?? "",
                NamaKegiatan = anggaran.NmGiat ?? "",
                KodeOutput = anggaran.KdOutput ?? "",
                NamaOutput = anggaran.NmOutput ?? "",
                KodeSuboutput = anggaran.KdSOutput ?? "",
                NamaSuboutput = anggaran.NmSOutput ?? "",
                KodeKomponen = anggaran.KdKmpnen ?? "",
                NamaKomponen = anggaran.NmKmpnen ?? "",
                KodeSubkomponen = anggaran.KdSkmpnen ?? "",
                NamaSubkomponen = anggaran.NmSkmpnen ?? "",
                KodeAkun = anggaran.KdAkun ?? "",
                NamaAkun = anggaran.NmAkun ?? "",
                NoItem = anggaran.NoItem ?? "",
                NamaItem = anggaran.NmItem ?? "",
                PaguAnggaran = pagu,
                Realisasi = realisasi,
                SisaAnggaran = sisa,
                PersenRealisasi = persen,
                TahunAnggaran = tahun,
                Revisi = revisiTerakhir
            };
        }).ToList();

        return result;
    }
}
