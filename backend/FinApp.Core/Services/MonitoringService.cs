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

    public async Task<IEnumerable<MonitoringAnggaranDto>> GetMonitoringAnggaranAsync(int tahun, Guid userId)
    {
        // Get user with role and suboutput assignments
        var user = await _unitOfWork.Users.GetByIdWithRoleAsync(userId);
        if (user == null)
        {
            return Enumerable.Empty<MonitoringAnggaranDto>();
        }

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

        // Filter by user role and assigned suboutputs
        // Admin, PPK, and Bendahara can see all data
        if (user.Role.Name != "Admin" && user.Role.Name != "PPK" && user.Role.Name != "Bendahara")
        {
            var userSuboutputs = user.Role.RoleSuboutputs
                .Select(rs => rs.KodeSuboutput)
                .ToHashSet();

            if (userSuboutputs.Any())
            {
                anggaranData = anggaranData
                    .Where(a => userSuboutputs.Contains(a.KdSOutput ?? ""))
                    .ToList();
            }
            else
            {
                // User has no assigned suboutputs, return empty
                return Enumerable.Empty<MonitoringAnggaranDto>();
            }
        }

        // Get realisasi from all STPB details (regardless of status) for the year
        var allStpbDetails = await _unitOfWork.StpbDetails.GetAllAsync();
        var allStpbs = await _unitOfWork.Stpbs.GetAllAsync();
        
        var stpbIdsForYear = allStpbs
            .Where(s => s.Tahun == tahun)
            .Select(s => s.Id)
            .ToHashSet();

        var realisasiData = allStpbDetails
            .Where(sd => stpbIdsForYear.Contains(sd.StpbId))
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
                TotalRealisasi = g.Sum(sd => sd.JumlahHarga)
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

    public async Task<IEnumerable<StpbDetailMonitoringDto>> GetStpbDetailsAsync(
        string kodeProgram,
        string kodeKegiatan,
        string kodeOutput,
        string kodeSuboutput,
        string kodeKomponen,
        string kodeSubkomponen,
        string kodeAkun,
        string noItem,
        int tahun,
        Guid userId)
    {
        // Get user with role for access control
        var user = await _unitOfWork.Users.GetByIdWithRoleAsync(userId);
        if (user == null)
        {
            return Enumerable.Empty<StpbDetailMonitoringDto>();
        }

        // Filter by user role and assigned suboutputs
        if (user.Role.Name != "Admin")
        {
            var userSuboutputs = user.Role.RoleSuboutputs
                .Select(rs => rs.KodeSuboutput)
                .ToHashSet();

            if (!userSuboutputs.Contains(kodeSuboutput))
            {
                // User doesn't have access to this suboutput
                return Enumerable.Empty<StpbDetailMonitoringDto>();
            }
        }

        // Get all STPB for the year (regardless of status)
        var allStpbs = await _unitOfWork.Stpbs.GetAllAsync();
        var stpbsForYear = allStpbs
            .Where(s => s.Tahun == tahun)
            .ToList();

        var stpbIdsForYear = stpbsForYear.Select(s => s.Id).ToHashSet();

        // Get STPB details matching the anggaran
        var allStpbDetails = await _unitOfWork.StpbDetails.GetAllAsync();
        var matchingDetails = allStpbDetails
            .Where(sd => 
                stpbIdsForYear.Contains(sd.StpbId) &&
                sd.KodeProgram == kodeProgram &&
                sd.KodeKegiatan == kodeKegiatan &&
                sd.KodeOutput == kodeOutput &&
                sd.KodeSuboutput == kodeSuboutput &&
                sd.KodeKomponen == kodeKomponen &&
                sd.KodeSubkomponen == kodeSubkomponen &&
                sd.KodeAkun == kodeAkun &&
                sd.NoItem == noItem)
            .ToList();

        // Map to DTO with STPB header info
        var result = matchingDetails.Select(detail => 
        {
            var stpb = stpbsForYear.First(s => s.Id == detail.StpbId);
            var totalPajak = detail.PPN + detail.PPH21 + detail.PPH22 + detail.PPH23;
            return new StpbDetailMonitoringDto
            {
                StpbId = stpb.Id,
                NoStpb = stpb.NomorSTPB,
                TanggalStpb = detail.TanggalTransaksi,
                Keterangan = detail.Keterangan ?? detail.NamaItem ?? "-",
                Penerima = detail.Penerima,
                NilaiKotor = detail.JumlahHarga,
                Pajak = totalPajak,
                NilaiBersih = detail.NilaiBersih,
                Status = stpb.Status.ToString()
            };
        }).ToList();

        return result;
    }
}
