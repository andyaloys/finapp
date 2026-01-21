using FinApp.Domain.Entities;

namespace FinApp.Domain.Interfaces;

public interface IItemRepository : IRepository<Item>
{
    Task<(IEnumerable<Item> Items, int TotalCount)> GetPagedAsync(int pageNumber, int pageSize, string? searchTerm);
    Task<IEnumerable<Item>> GetByAkunIdAsync(Guid akunId);
}
