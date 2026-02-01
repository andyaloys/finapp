using FinApp.Core.DTOs.Monitoring;

namespace FinApp.Core.Interfaces;

public interface IMonitoringService
{
    Task<IEnumerable<MonitoringAnggaranDto>> GetMonitoringAnggaranAsync(int tahun);
}
