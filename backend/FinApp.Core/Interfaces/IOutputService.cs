using FinApp.Core.DTOs.Common;
using FinApp.Core.DTOs.Output;

namespace FinApp.Core.Interfaces;

public interface IOutputService
{
    Task<PagedResult<OutputDto>> GetAllAsync(int pageNumber, int pageSize, string? searchTerm);
    Task<OutputDto> GetByIdAsync(Guid id);
    Task<IEnumerable<OutputDto>> GetByKegiatanIdAsync(Guid kegiatanId);
    Task<OutputDto> CreateAsync(CreateOutputDto createDto);
    Task<OutputDto> UpdateAsync(Guid id, UpdateOutputDto updateDto);
    Task DeleteAsync(Guid id);
}
