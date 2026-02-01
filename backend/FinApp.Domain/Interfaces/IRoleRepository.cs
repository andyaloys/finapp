using FinApp.Domain.Entities;

namespace FinApp.Domain.Interfaces;

public interface IRoleRepository : IRepository<Role>
{
    Task<Role?> GetByNameAsync(string name);
    Task<Role?> GetByIdWithSuboutputsAsync(Guid id);
    Task<(IEnumerable<Role> items, int totalCount)> GetPagedAsync(int pageNumber, int pageSize, string? searchTerm);
}
