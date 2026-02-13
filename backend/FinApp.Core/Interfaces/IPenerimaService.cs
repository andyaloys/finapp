using FinApp.Core.DTOs.Penerima;

namespace FinApp.Core.Interfaces;

public interface IPenerimaService
{
    Task<IEnumerable<PenerimaDto>> GetAllAsync();
    Task<IEnumerable<PenerimaDto>> GetAllActiveAsync();
    Task<PenerimaDto?> GetByIdAsync(int id);
    Task<PenerimaDto> CreateAsync(CreatePenerimaDto dto);
    Task<PenerimaDto> UpdateAsync(int id, UpdatePenerimaDto dto);
    Task DeleteAsync(int id);
}
