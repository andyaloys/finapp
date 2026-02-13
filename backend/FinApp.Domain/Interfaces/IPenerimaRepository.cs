using FinApp.Domain.Entities;

namespace FinApp.Domain.Interfaces;

public interface IPenerimaRepository
{
    Task<Penerima?> GetByIdAsync(int id);
    Task<IEnumerable<Penerima>> GetAllAsync();
    Task<IEnumerable<Penerima>> GetAllActiveAsync();
    Task<Penerima?> GetByNamaAsync(string nama);
    Task<bool> ExistsByNamaAsync(string nama, int? excludeId = null);
    Task AddAsync(Penerima penerima);
    void Update(Penerima penerima);
    void Delete(Penerima penerima);
    Task SaveChangesAsync();
}
