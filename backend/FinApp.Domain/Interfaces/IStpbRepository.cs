using FinApp.Domain.Entities;

namespace FinApp.Domain.Interfaces;

public interface IStpbRepository : IRepository<Stpb>
{
    Task<IEnumerable<Stpb>> GetByUserIdAsync(int userId);
    Task<Stpb?> GetByNomorAsync(string nomorStpb);
    Task<IEnumerable<Stpb>> GetByStatusAsync(string status);
    Task<(IEnumerable<Stpb> Items, int TotalCount)> GetPagedAsync(int pageNumber, int pageSize, string? searchTerm = null);
}
