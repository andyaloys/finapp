using FinApp.Core.DTOs.Common;
using FinApp.Core.DTOs.Stpb;

namespace FinApp.Core.Interfaces;

public interface IStpbService
{
    Task<PagedResult<StpbDto>> GetAllAsync(int pageNumber, int pageSize, string? searchTerm = null);
    Task<StpbDto?> GetByIdAsync(int id);
    Task<StpbDto> CreateAsync(CreateStpbDto dto, int userId);
    Task<StpbDto> UpdateAsync(int id, UpdateStpbDto dto);
    Task<bool> DeleteAsync(int id);
    Task<IEnumerable<StpbDto>> GetByUserIdAsync(int userId);
}
