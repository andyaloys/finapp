using FinApp.Core.DTOs.Program;

namespace FinApp.Core.Interfaces
{
    public interface IProgramService
    {
        Task<IEnumerable<ProgramDto>> GetAllAsync();
        Task<ProgramDto> GetByIdAsync(Guid id);
        Task<ProgramDto> CreateAsync(CreateProgramDto dto);
        Task<ProgramDto> UpdateAsync(Guid id, UpdateProgramDto dto);
        Task DeleteAsync(Guid id);
    }
}
