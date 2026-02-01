namespace FinApp.Domain.Interfaces;

public interface IUnitOfWork : IDisposable
{
    IUserRepository Users { get; }
    IRoleRepository Roles { get; }
    IRoleSuboutputRepository RoleSuboutputs { get; }
    IPpkBendaharaRepository PpkBendaharas { get; }
    IStpbRepository Stpbs { get; }
    IStpbDetailRepository StpbDetails { get; }
    IProgramRepository Programs { get; }
    IKegiatanRepository Kegiatans { get; }
    IOutputRepository Outputs { get; }
    ISuboutputRepository Suboutputs { get; }
    IKomponenRepository Komponens { get; }
    ISubkomponenRepository Subkomponens { get; }
    IAkunRepository Akuns { get; }
    IItemRepository Items { get; }
    ISequenceNumberRepository SequenceNumbers { get; }
    IAnggaranMasterRepository AnggaranMasters { get; }
    Task<int> SaveChangesAsync();
    Task BeginTransactionAsync();
    Task CommitTransactionAsync();
    Task RollbackTransactionAsync();
}
