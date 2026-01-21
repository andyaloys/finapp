using FinApp.Core.DTOs.Common;
using FinApp.Core.DTOs.Subkomponen;

namespace FinApp.Core.Interfaces;

public interface ISubkomponenService
{
    Task<PagedResult<SubkomponenDto>> GetAllAsync(int pageNumber, int pageSize, string? searchTerm);
    Task<SubkomponenDto> GetByIdAsync(Guid id);
    Task<IEnumerable<SubkomponenDto>> GetByKomponenIdAsync(Guid komponenId);
    Task<SubkomponenDto> CreateAsync(CreateSubkomponenDto createDto);
    Task<SubkomponenDto> UpdateAsync(Guid id, UpdateSubkomponenDto updateDto);
    Task DeleteAsync(Guid id);
}
