using FinApp.Domain.Entities;

namespace FinApp.Domain.Interfaces;

public interface IAkunRepository : IRepository<Akun>
{
    Task<(IEnumerable<Akun> Items, int TotalCount)> GetPagedAsync(int pageNumber, int pageSize, string? searchTerm);
    Task<IEnumerable<Akun>> GetBySubkomponenIdAsync(Guid subkomponenId);
}
