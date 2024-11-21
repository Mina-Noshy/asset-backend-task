using Asset.Domain.Entities.Shared;
using System.Linq.Expressions;

namespace Asset.Domain.Interfaces.Common;

public interface IRepository
{
    // Dynamic Collection Without Pagination
    Task<IEnumerable<object>> GetAllDynamicAsync<T>(Expression<Func<T, bool>>? filterExpression,
           Expression<Func<T, object>> selectionExpression,
           Func<IQueryable<T>, IOrderedQueryable<T>>? orderByExpression = null,
           CancellationToken cancellationToken = default) where T : TEntity;

    Task<IEnumerable<TProjection>> GetAllDynamicAsync<T, TProjection>(Expression<Func<T, bool>>? filterExpression,
        Expression<Func<T, TProjection>> selectionExpression,
        Func<IQueryable<T>, IOrderedQueryable<T>>? orderByExpression = null,
        CancellationToken cancellationToken = default) where T : TEntity;


    /****************************************************************************************************************/


    // Dynamic Collection With Pagination
    Task<IEnumerable<object>> GetDynamicAsync<T>(Expression<Func<T, bool>>? filterExpression,
            Expression<Func<T, object>> selectionExpression,
            Func<IQueryable<T>, IOrderedQueryable<T>>? orderByExpression = null,
            int? pageNumber = null,
            int? pageSize = null,
            CancellationToken cancellationToken = default) where T : TEntity;

    Task<IEnumerable<TProjection>> GetDynamicAsync<T, TProjection>(Expression<Func<T, bool>>? filterExpression,
        Expression<Func<T, TProjection>> selectionExpression,
        Func<IQueryable<T>, IOrderedQueryable<T>>? orderByExpression = null,
        int? pageNumber = null,
        int? pageSize = null,
        CancellationToken cancellationToken = default) where T : TEntity;



    /****************************************************************************************************************/



    // Entity All Collection Without Pagination
    IQueryable<T> GetAll<T>(Expression<Func<T, bool>> filterExpression,
        Func<IQueryable<T>, IOrderedQueryable<T>>? orderByExpression = null,
        params string[] includeProperties) where T : TEntity;

    Task<IEnumerable<T>> GetAllAsync<T>(Expression<Func<T, bool>> filterExpression,
        Func<IQueryable<T>, IOrderedQueryable<T>>? orderByExpression = null,
        CancellationToken cancellationToken = default,
        params string[] includeProperties) where T : TEntity;



    /****************************************************************************************************************/



    // Entity All Collection With Pagination
    IQueryable<T> Find<T>(Expression<Func<T, bool>> filterExpression,
        Func<IQueryable<T>, IOrderedQueryable<T>>? orderByExpression = null,
        int? pageNumber = null,
        int? pageSize = null,
        params string[] includeProperties) where T : TEntity;

    Task<IEnumerable<T>> FindAsync<T>(Expression<Func<T, bool>> filterExpression,
        Func<IQueryable<T>, IOrderedQueryable<T>>? orderByExpression = null,
        int? pageNumber = null,
        int? pageSize = null,
        CancellationToken cancellationToken = default,
        params string[] includeProperties) where T : TEntity;



    /****************************************************************************************************************/



    // Single Entity
    Task<T?> GetByIdAsync<T>(long id,
        CancellationToken cancellationToken = default,
        params string[] includeProperties) where T : TEntity;

    Task<T?> SingleOrDefaultAsync<T>(Expression<Func<T, bool>> filterExpression,
        CancellationToken cancellationToken = default,
        params string[] includeProperties) where T : TEntity;

    Task<T?> FirstOrDefaultAsync<T>(Expression<Func<T, bool>> filterExpression,
        CancellationToken cancellationToken = default,
        params string[] includeProperties) where T : TEntity;


    /****************************************************************************************************************/



    // Add Entity
    void Add<T>(T entity) where T : TEntity;

    Task AddRangeAsync<T>(IEnumerable<T> entities,
        CancellationToken cancellationToken = default) where T : TEntity;



    /****************************************************************************************************************/



    // Update Entity
    void Update<T>(T entity) where T : TEntity;

    void UpdateRange<T>(IEnumerable<T> entities) where T : TEntity;



    /****************************************************************************************************************/



    // Remove Entity
    void Remove<T>(T entity) where T : TEntity;

    void RemoveRange<T>(IEnumerable<T> entities) where T : TEntity;



    /****************************************************************************************************************/



    // Count Entity
    Task<int> CountAsync<T>(Expression<Func<T, bool>> filterExpression,
        CancellationToken cancellationToken = default) where T : TEntity;


    /****************************************************************************************************************/


    // Check Exists Entity
    Task<bool> AnyAsync<T>(Expression<Func<T, bool>> filterExpression,
        CancellationToken cancellationToken = default) where T : TEntity;
}
