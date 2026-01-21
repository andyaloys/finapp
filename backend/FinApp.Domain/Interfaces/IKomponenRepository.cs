using FinApp.Domain.Entities;

namespace FinApp.Domain.Interfaces;

public interface IKomponenRepository : IRepository<Komponen>
{
    Task<(IEnumerable<Komponen> Items, int TotalCount)> GetPagedAsync(int pageNumber, int pageSize, string? searchTerm);
    Task<IEnumerable<Komponen>> GetBySuboutputIdAsync(Guid suboutputId);
}
