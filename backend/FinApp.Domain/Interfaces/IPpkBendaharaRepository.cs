using FinApp.Domain.Entities;

namespace FinApp.Domain.Interfaces;

public interface IPpkBendaharaRepository : IRepository<PpkBendahara>
{
    Task<IEnumerable<PpkBendahara>> GetActiveAsync();
    Task<PpkBendahara?> GetByNIPAsync(string nip);
    Task<IEnumerable<PpkBendahara>> GetByJabatanAsync(JabatanType jabatan);
}
