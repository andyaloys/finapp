using FinApp.Core.DTOs.Monitoring;

namespace FinApp.Core.Interfaces;

public interface IMonitoringService
{
    Task<IEnumerable<MonitoringAnggaranDto>> GetMonitoringAnggaranAsync(int tahun, Guid userId);
    Task<IEnumerable<StpbDetailMonitoringDto>> GetStpbDetailsAsync(
        string kodeProgram,
        string kodeKegiatan,
        string kodeOutput,
        string kodeSuboutput,
        string kodeKomponen,
        string kodeSubkomponen,
        string kodeAkun,
        string noItem,
        int tahun,
        Guid userId);
}
