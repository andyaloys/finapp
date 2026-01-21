using FinApp.Core.DTOs.Common;
using FinApp.Core.DTOs.Komponen;

namespace FinApp.Core.Interfaces;

public interface IKomponenService
{
    Task<PagedResult<KomponenDto>> GetAllAsync(int pageNumber, int pageSize, string? searchTerm);
    Task<KomponenDto> GetByIdAsync(Guid id);
    Task<IEnumerable<KomponenDto>> GetBySuboutputIdAsync(Guid suboutputId);
    Task<KomponenDto> CreateAsync(CreateKomponenDto createDto);
    Task<KomponenDto> UpdateAsync(Guid id, UpdateKomponenDto updateDto);
    Task DeleteAsync(Guid id);
}
