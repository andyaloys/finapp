using FinApp.Domain.Entities;

namespace FinApp.Domain.Interfaces;

public interface IStpbDetailRepository : IRepository<StpbDetail>
{
    Task<IEnumerable<StpbDetail>> GetByStpbIdAsync(Guid stpbId);
    Task<IEnumerable<StpbDetail>> GetBySuboutputAsync(string kodeSuboutput);
}
