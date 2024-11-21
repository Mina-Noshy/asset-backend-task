using Asset.Domain.Interfaces.Common;
using Asset.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Asset.Infrastructure.Persistence;

internal class UnitOfWork : IUnitOfWork
{
    private readonly IMainDbContext _context;

    public UnitOfWork(IMainDbContext context, IDatabaseRouter databaseRouter)
    {
        context.Database.SetDbConnection(databaseRouter.GetSqlConnection());
        _context = context;
    }

    public IRepository Repository()
        => new Repository(_context);

    public async Task<int> CommitAsync(CancellationToken cancellationToken = default)
        => await _context.SaveChangesAsync(cancellationToken);

    public async Task BeginTransactionAsync(CancellationToken cancellationToken = default)
        => await _context.Database.BeginTransactionAsync(cancellationToken);

    public async Task CommitTransactionAsync(CancellationToken cancellationToken = default)
        => await _context.Database.CommitTransactionAsync(cancellationToken);

    public async Task RollbackTransactionAsync(CancellationToken cancellationToken = default)
        => await _context.Database.RollbackTransactionAsync(cancellationToken);

}
