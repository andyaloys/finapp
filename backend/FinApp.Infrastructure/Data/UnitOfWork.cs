using FinApp.Domain.Interfaces;
using FinApp.Infrastructure.Data;
using FinApp.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore.Storage;

namespace FinApp.Infrastructure.Data;

public class UnitOfWork : IUnitOfWork
{
    private readonly AppDbContext _context;
    private IDbContextTransaction? _transaction;
    
    public IUserRepository Users { get; }
    public IStpbRepository Stpbs { get; }
    public IProgramRepository Programs { get; }
    public IKegiatanRepository Kegiatans { get; }
    public IOutputRepository Outputs { get; }
    public ISuboutputRepository Suboutputs { get; }
    public IKomponenRepository Komponens { get; }
    public ISubkomponenRepository Subkomponens { get; }
    public IAkunRepository Akuns { get; }
    public IItemRepository Items { get; }
    public ISequenceNumberRepository SequenceNumbers { get; }

    public UnitOfWork(AppDbContext context)
    {
        _context = context;
        Users = new UserRepository(context);
        Stpbs = new StpbRepository(context);
        Programs = new ProgramRepository(context);
        Kegiatans = new KegiatanRepository(context);
        Outputs = new OutputRepository(context);
        Suboutputs = new SuboutputRepository(context);
        Komponens = new KomponenRepository(context);
        Subkomponens = new SubkomponenRepository(context);
        Akuns = new AkunRepository(context);
        Items = new ItemRepository(context);
        SequenceNumbers = new SequenceNumberRepository(context);
    }

    public async Task<int> SaveChangesAsync()
    {
        return await _context.SaveChangesAsync();
    }

    public async Task BeginTransactionAsync()
    {
        _transaction = await _context.Database.BeginTransactionAsync();
    }

    public async Task CommitTransactionAsync()
    {
        try
        {
            await _context.SaveChangesAsync();
            if (_transaction != null)
            {
                await _transaction.CommitAsync();
            }
        }
        catch
        {
            await RollbackTransactionAsync();
            throw;
        }
        finally
        {
            if (_transaction != null)
            {
                await _transaction.DisposeAsync();
                _transaction = null;
            }
        }
    }

    public async Task RollbackTransactionAsync()
    {
        if (_transaction != null)
        {
            await _transaction.RollbackAsync();
            await _transaction.DisposeAsync();
            _transaction = null;
        }
    }

    public void Dispose()
    {
        _transaction?.Dispose();
        _context.Dispose();
    }
}
