using FinApp.Domain.Entities;

namespace FinApp.Domain.Interfaces;

public interface IMenuRepository : IRepository<Menu>
{
    Task<List<Menu>> GetAllActiveAsync();
    Task<Menu?> GetByKeyAsync(string key);
}
