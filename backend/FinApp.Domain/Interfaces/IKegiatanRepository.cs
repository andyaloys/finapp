using FinApp.Domain.Entities;

namespace FinApp.Domain.Interfaces;

public interface IKegiatanRepository : IRepository<Kegiatan>
{
    Task<(IEnumerable<Kegiatan> Items, int TotalCount)> GetPagedAsync(int pageNumber, int pageSize, string? searchTerm);
    Task<IEnumerable<Kegiatan>> GetByProgramIdAsync(Guid programId);
}
