using FinApp.Core.DTOs.PpkBendahara;

namespace FinApp.Core.Interfaces;

public interface IPpkBendaharaService
{
    Task<IEnumerable<PpkBendaharaDto>> GetAllAsync();
    Task<IEnumerable<PpkBendaharaDto>> GetActiveAsync();
    Task<PpkBendaharaDto?> GetByIdAsync(Guid id);
    Task<PpkBendaharaDto> CreateAsync(CreatePpkBendaharaDto dto);
    Task<PpkBendaharaDto> UpdateAsync(Guid id, UpdatePpkBendaharaDto dto);
    Task<bool> DeleteAsync(Guid id);
}
