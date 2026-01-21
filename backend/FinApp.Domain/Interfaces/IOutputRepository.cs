using FinApp.Domain.Entities;

namespace FinApp.Domain.Interfaces;

public interface IOutputRepository : IRepository<Output>
{
    Task<(IEnumerable<Output> Items, int TotalCount)> GetPagedAsync(int pageNumber, int pageSize, string? searchTerm);
    Task<IEnumerable<Output>> GetByKegiatanIdAsync(Guid kegiatanId);
}
