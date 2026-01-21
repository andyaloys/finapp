using FinApp.Core.DTOs.Common;
using FinApp.Core.DTOs.Suboutput;

namespace FinApp.Core.Interfaces;

public interface ISuboutputService
{
    Task<PagedResult<SuboutputDto>> GetAllAsync(int pageNumber, int pageSize, string? searchTerm);
    Task<SuboutputDto> GetByIdAsync(Guid id);
    Task<IEnumerable<SuboutputDto>> GetByOutputIdAsync(Guid outputId);
    Task<SuboutputDto> CreateAsync(CreateSuboutputDto createDto);
    Task<SuboutputDto> UpdateAsync(Guid id, UpdateSuboutputDto updateDto);
    Task DeleteAsync(Guid id);
}
