using FinApp.Domain.Entities;

namespace FinApp.Domain.Interfaces;

public interface IProgramRepository : IRepository<Program>
{
    Task<Program?> GetByKodeProgramAsync(string kodeProgram);
}
