using FinApp.Domain.Entities;
using FinApp.Domain.Interfaces;
using FinApp.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace FinApp.Infrastructure.Repositories;

public class SequenceNumberRepository : Repository<SequenceNumber>, ISequenceNumberRepository
{
    public SequenceNumberRepository(AppDbContext context) : base(context)
    {
    }

    public async Task<int> GetNextNumberAsync(string entityType, int year)
    {
        // Start transaction with serializable isolation to prevent race condition
        using var transaction = await _context.Database.BeginTransactionAsync(System.Data.IsolationLevel.Serializable);
        
        try
        {
            var sequence = await _context.Set<SequenceNumber>()
                .FirstOrDefaultAsync(s => s.EntityType == entityType && s.Year == year);

            if (sequence == null)
            {
                // Create new sequence for this year
                sequence = new SequenceNumber
                {
                    EntityType = entityType,
                    Year = year,
                    LastNumber = 1
                };
                await _context.Set<SequenceNumber>().AddAsync(sequence);
            }
            else
            {
                // Increment existing sequence
                sequence.LastNumber++;
                _context.Set<SequenceNumber>().Update(sequence);
            }

            await _context.SaveChangesAsync();
            await transaction.CommitAsync();

            return sequence.LastNumber;
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }
    }
}
