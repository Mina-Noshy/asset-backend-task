namespace Asset.Domain.Interfaces.Common;

public interface IUnitOfWork
{
    IRepository Repository();
    Task<int> CommitAsync(CancellationToken cancellationToken);
    Task BeginTransactionAsync(CancellationToken cancellationToken = default);
    Task CommitTransactionAsync(CancellationToken cancellationToken = default);
    Task RollbackTransactionAsync(CancellationToken cancellationToken = default);
}
