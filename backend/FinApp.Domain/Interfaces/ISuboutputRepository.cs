using FinApp.Domain.Entities;

namespace FinApp.Domain.Interfaces;

public interface ISuboutputRepository : IRepository<Suboutput>
{
    Task<(IEnumerable<Suboutput> Items, int TotalCount)> GetPagedAsync(int pageNumber, int pageSize, string? searchTerm);
    Task<IEnumerable<Suboutput>> GetByOutputIdAsync(Guid outputId);
}
