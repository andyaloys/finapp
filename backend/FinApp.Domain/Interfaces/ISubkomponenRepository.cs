using FinApp.Domain.Entities;

namespace FinApp.Domain.Interfaces;

public interface ISubkomponenRepository : IRepository<Subkomponen>
{
    Task<(IEnumerable<Subkomponen> Items, int TotalCount)> GetPagedAsync(int pageNumber, int pageSize, string? searchTerm);
    Task<IEnumerable<Subkomponen>> GetByKomponenIdAsync(Guid komponenId);
}
