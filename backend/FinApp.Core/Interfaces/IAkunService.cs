using FinApp.Core.DTOs.Akun;
using FinApp.Core.DTOs.Common;

namespace FinApp.Core.Interfaces;

public interface IAkunService
{
    Task<PagedResult<AkunDto>> GetAllAsync(int pageNumber, int pageSize, string? searchTerm);
    Task<AkunDto> GetByIdAsync(Guid id);
    Task<IEnumerable<AkunDto>> GetBySubkomponenIdAsync(Guid subkomponenId);
    Task<AkunDto> CreateAsync(CreateAkunDto createDto);
    Task<AkunDto> UpdateAsync(Guid id, UpdateAkunDto updateDto);
    Task DeleteAsync(Guid id);
}
