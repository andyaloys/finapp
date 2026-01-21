using FinApp.Core.DTOs.Common;
using FinApp.Core.DTOs.Kegiatan;

namespace FinApp.Core.Interfaces;

public interface IKegiatanService
{
    Task<PagedResult<KegiatanDto>> GetAllAsync(int pageNumber, int pageSize, string? searchTerm);
    Task<KegiatanDto> GetByIdAsync(Guid id);
    Task<IEnumerable<KegiatanDto>> GetByProgramIdAsync(Guid programId);
    Task<KegiatanDto> CreateAsync(CreateKegiatanDto createDto);
    Task<KegiatanDto> UpdateAsync(Guid id, UpdateKegiatanDto updateDto);
    Task DeleteAsync(Guid id);
}
