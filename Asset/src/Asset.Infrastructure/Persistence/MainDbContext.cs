using Asset.Domain.Entities.Auth;
using Asset.Domain.Entities.Auth.Identity;
using Asset.Domain.Entities.BookInventory;
using Asset.Domain.Entities.Shared;
using Asset.Domain.Interfaces.Common;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Serilog;

namespace Asset.Infrastructure.Persistence;

public class MainDbContext :
    IdentityDbContext
        <
        UserMaster,
        RoleMaster,
        long,
        UserClaimsMaster,
        UserRoleMaster,
        UserLoginsMaster,
        RoleClaimsMaster,
        UserTokensMaster
        >,
    IMainDbContext
{

    public MainDbContext(DbContextOptions<MainDbContext> _options) : base(_options) { }


    private readonly IDateTimeProvider _dateTimeProvider;
    private readonly ICurrentUser _currentUser;

    public MainDbContext(DbContextOptions<MainDbContext> _options,
        IDateTimeProvider dateTimeProvider,
        ICurrentUser currentUser) : base(_options)
    {
        _dateTimeProvider = dateTimeProvider;
        _currentUser = currentUser;
    }



    #region Auth Entities
    public DbSet<CompanyMaster> CompanyMaster { get; set; }
    #endregion

    #region BookInventory Entities
    public DbSet<BookCategory> BookCategories { get; set; }
    public DbSet<BookMaster> BookMaster { get; set; }
    public DbSet<BookTransaction> BookTransactions { get; set; }
    #endregion




    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default(CancellationToken))
    {
        foreach (var entry in ChangeTracker.Entries<TEntity>().AsEnumerable())
        {
            if (entry.State == EntityState.Added)
            {
                entry.Entity.CreatedBy = _currentUser.Username;
                entry.Entity.CreatedAt = _dateTimeProvider.CurrentDateTime;
                Log.Information("Entity '{EntityType}' with ID '{EntityId}' was added by '{UserName}' at '{Timestamp}'", entry.Entity.GetType().Name, entry.Entity.Id, _currentUser.Username, _dateTimeProvider.CurrentDateTime);
            }
            else if (entry.State == EntityState.Modified)
            {
                // We don't delete any entity from db
                // We just switch 'IsDeleted' to 'True'

                if (entry.Entity.IsDeleted)
                {
                    entry.Entity.DeletedBy = _currentUser.Username;
                    entry.Entity.DeletedAt = _dateTimeProvider.CurrentDateTime;
                }
                else
                {
                    entry.Entity.ModifiedBy = _currentUser.Username;
                    entry.Entity.ModifiedAt = _dateTimeProvider.CurrentDateTime;
                }
                Log.Information("Entity '{EntityType}' with ID '{EntityId}' was modified by '{UserName}' at '{Timestamp}'", entry.Entity.GetType().Name, entry.Entity.Id, _currentUser.Username, _dateTimeProvider.CurrentDateTime);
            }
            else if (entry.State == EntityState.Deleted)
            {
                Log.Information("Entity '{EntityType}' with ID '{EntityId}' was deleted by '{UserName}' at '{Timestamp}'", entry.Entity.GetType().Name, entry.Entity.Id, _currentUser.Username, _dateTimeProvider.CurrentDateTime);
            }
        }

        return await base.SaveChangesAsync(cancellationToken);
    }

    public async Task<IDbContextTransaction> BeginTransactionAsync(CancellationToken cancellationToken)
    {
        return await Database.BeginTransactionAsync(cancellationToken);
    }

    public async Task CommitTransactionAsync(IDbContextTransaction transaction)
    {
        await transaction.CommitAsync();
    }

    public async Task RollbackTransactionAsync(IDbContextTransaction transaction)
    {
        await transaction.RollbackAsync();
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Configure customizations for Identity tables
        modelBuilder.Entity<UserMaster>().ToTable("UserMaster");
        modelBuilder.Entity<RoleMaster>().ToTable("RoleMaster");
        modelBuilder.Entity<UserRoleMaster>().ToTable("UserRoleMaster");
        modelBuilder.Entity<UserClaimsMaster>().ToTable("UserClaimsMaster");
        modelBuilder.Entity<UserLoginsMaster>().ToTable("UserLoginsMaster");
        modelBuilder.Entity<RoleClaimsMaster>().ToTable("RoleClaimsMaster");
        modelBuilder.Entity<UserTokensMaster>().ToTable("UserTokensMaster");

        // Apply all entities configuration
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(MainDbContext).Assembly);

    }

}
