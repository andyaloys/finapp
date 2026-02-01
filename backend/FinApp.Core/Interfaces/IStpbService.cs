using FinApp.Core.DTOs.Common;
using FinApp.Core.DTOs.Stpb;

namespace FinApp.Core.Interfaces;

public interface IStpbService
{
    Task<PagedResult<StpbDto>> GetAllAsync(int pageNumber, int pageSize, string? searchTerm, Guid userId);
    Task<StpbDto?> GetByIdAsync(Guid id);
    Task<StpbDto> CreateAsync(CreateStpbDto dto, Guid userId);
    Task<StpbDto> UpdateAsync(Guid id, UpdateStpbDto dto);
    Task<bool> DeleteAsync(Guid id);
    Task<IEnumerable<StpbDto>> GetByUserIdAsync(Guid userId);
    
    // Workflow methods
    Task<StpbDto> KirimAsync(Guid id, Guid userId);
    Task<StpbDto> ApproveAsync(Guid id, Guid userId);
    Task<StpbDto> KembalikanAsync(Guid id, Guid userId, string alasan);
    
    // Detail management methods
    Task<StpbDetailDto> AddDetailAsync(Guid stpbId, CreateStpbDetailDto dto, Guid userId);
    Task<StpbDetailDto> UpdateDetailAsync(Guid stpbId, Guid detailId, CreateStpbDetailDto dto, Guid userId);
    Task<bool> DeleteDetailAsync(Guid stpbId, Guid detailId, Guid userId);
}

