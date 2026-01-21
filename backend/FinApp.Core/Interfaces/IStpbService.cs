using FinApp.Core.DTOs.Common;
using FinApp.Core.DTOs.Stpb;

namespace FinApp.Core.Interfaces;

public interface IStpbService
{
    Task<PagedResult<StpbDto>> GetAllAsync(int pageNumber, int pageSize, string? searchTerm = null);
    Task<StpbDto?> GetByIdAsync(Guid id);
    Task<StpbDto> CreateAsync(CreateStpbDto dto, Guid userId);
    Task<StpbDto> UpdateAsync(Guid id, UpdateStpbDto dto);
    Task<bool> DeleteAsync(Guid id);
    Task<IEnumerable<StpbDto>> GetByUserIdAsync(Guid userId);
}
