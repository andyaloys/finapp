using FinApp.Domain.Entities;

namespace FinApp.Domain.Interfaces;

public interface IAnggaranMasterRepository : IRepository<AnggaranMaster>
{
    Task<List<AnggaranMaster>> GetByTahunRevisiAsync(int tahun, int revisi);
    Task<int> GetLastRevisiAsync(int tahun);
}
