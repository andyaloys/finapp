namespace FinApp.Domain.Interfaces;

public interface ISequenceNumberRepository : IRepository<Entities.SequenceNumber>
{
    Task<int> GetNextNumberAsync(string entityType, int year);
}
