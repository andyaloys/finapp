using FinApp.Domain.Entities;

namespace FinApp.Domain.Interfaces;

public interface IStpbDetailRepository : IRepository<StpbDetail>
{
    Task<IEnumerable<StpbDetail>> GetByStpbIdAsync(Guid stpbId);
    Task<IEnumerable<StpbDetail>> GetBySuboutputAsync(string kodeSuboutput);
    Task<decimal> GetRealisasiByItemAsync(
        int tahun, 
        int revisi, 
        string kdProgram, 
        string kdGiat, 
        string kdOutput, 
        string kdSOutput, 
        string kdKmpnen, 
        string kdSkmpnen, 
        string kdAkun, 
        string noItem);
}
