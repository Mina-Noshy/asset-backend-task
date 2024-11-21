using Asset.Domain.Entities.Shared;
using Asset.Domain.Interfaces.Common;
using Asset.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Asset.Infrastructure.Repositories;

internal class Repository(IMainDbContext _context) : IRepository
{
    // Dynamic Collection Without Pagination
    public async Task<IEnumerable<object>> GetAllDynamicAsync<T>(Expression<Func<T, bool>>? filterExpression,
        Expression<Func<T, object>> selectionExpression,
        Func<IQueryable<T>, IOrderedQueryable<T>>? orderByExpression = null,
        CancellationToken cancellationToken = default) where T : TEntity
    {
        IQueryable<T> query = _context.Set<T>();

        // Apply filterExpression if provided
        if (filterExpression != null)
            query = query.Where(filterExpression);

        // Apply additional conditions for IsDeleted
        query = query.Where(x => x.IsDeleted == false);

        // Apply orderByExpression if provided
        if (orderByExpression == null)
        {
            orderByExpression = x => x.OrderByDescending(s => s.Id);
        }
        query = orderByExpression(query);

        // Select only the required properties
        var selectedQuery = query.Select(selectionExpression);

        return await selectedQuery.ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<TProjection>> GetAllDynamicAsync<T, TProjection>(Expression<Func<T, bool>>? filterExpression,
        Expression<Func<T, TProjection>> selectionExpression,
        Func<IQueryable<T>, IOrderedQueryable<T>>? orderByExpression = null,
        CancellationToken cancellationToken = default) where T : TEntity
    {
        IQueryable<T> query = _context.Set<T>();

        // Apply filterExpression if provided
        if (filterExpression != null)
            query = query.Where(filterExpression);

        // Apply additional conditions for IsDeleted
        query = query.Where(x => x.IsDeleted == false);

        // Apply orderByExpression if provided
        if (orderByExpression == null)
        {
            orderByExpression = x => x.OrderByDescending(s => s.Id);
        }
        query = orderByExpression(query);

        // Select only the required properties
        var selectedQuery = query.Select(selectionExpression);

        // Apply skip and take
        return await selectedQuery.Distinct().ToListAsync(cancellationToken);
    }


    /****************************************************************************************************************/


    // Dynamic Collection With Pagination
    public async Task<IEnumerable<object>> GetDynamicAsync<T>(Expression<Func<T, bool>>? filterExpression,
        Expression<Func<T, object>> selectionExpression,
        Func<IQueryable<T>, IOrderedQueryable<T>>? orderByExpression = null,
        int? pageNumber = null,
        int? pageSize = null,
        CancellationToken cancellationToken = default) where T : TEntity
    {
        IQueryable<T> query = _context.Set<T>();

        // Apply filterExpression if provided
        if (filterExpression != null)
            query = query.Where(filterExpression);

        // Apply additional conditions for IsDeleted
        query = query.Where(x => x.IsDeleted == false);

        // Apply orderByExpression if provided
        if (orderByExpression == null)
        {
            orderByExpression = x => x.OrderByDescending(s => s.Id);
        }
        query = orderByExpression(query);

        // Apply pagination
        query = ApplyPagination(query, pageNumber, pageSize);

        // Select only the required properties
        var selectedQuery = query.Select(selectionExpression);

        return await selectedQuery.ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<TProjection>> GetDynamicAsync<T, TProjection>(Expression<Func<T, bool>>? filterExpression,
        Expression<Func<T, TProjection>> selectionExpression,
        Func<IQueryable<T>, IOrderedQueryable<T>>? orderByExpression = null,
        int? pageNumber = null,
        int? pageSize = null,
        CancellationToken cancellationToken = default) where T : TEntity
    {
        IQueryable<T> query = _context.Set<T>();

        // Apply filterExpression if provided
        if (filterExpression != null)
            query = query.Where(filterExpression);

        // Apply additional conditions for IsDeleted
        query = query.Where(x => x.IsDeleted == false);

        // Apply orderByExpression if provided
        if (orderByExpression == null)
        {
            orderByExpression = x => x.OrderByDescending(s => s.Id);
        }
        query = orderByExpression(query);

        // Apply pagination
        query = ApplyPagination(query, pageNumber, pageSize);

        // Select only the required properties
        var selectedQuery = query.Select(selectionExpression);

        // Apply skip and take
        return await selectedQuery.Distinct().ToListAsync(cancellationToken);
    }



    /****************************************************************************************************************/




    // Entity All Collection Without Pagination
    public IQueryable<T> GetAll<T>(Expression<Func<T, bool>> filterExpression,
        Func<IQueryable<T>, IOrderedQueryable<T>>? orderByExpression = null,
        params string[] includeProperties) where T : TEntity
    {
        IQueryable<T> query = _context.Set<T>().Where(filterExpression);

        // Apply additional conditions for IsDeleted
        query = query.Where(x => x.IsDeleted == false);

        // Include properties
        query = includeProperties.Aggregate(query, (current, includeProperty) => current.Include(includeProperty));

        // Apply orderByExpression if provided
        if (orderByExpression == null)
        {
            orderByExpression = x => x.OrderByDescending(s => s.Id);
        }
        query = orderByExpression(query);

        return query;
    }

    public async Task<IEnumerable<T>> GetAllAsync<T>(Expression<Func<T, bool>> filterExpression,
        Func<IQueryable<T>, IOrderedQueryable<T>>? orderByExpression = null,
        CancellationToken cancellationToken = default,
        params string[] includeProperties) where T : TEntity
    {
        IQueryable<T> query = _context.Set<T>().Where(filterExpression);

        // Apply additional conditions for IsDeleted
        query = query.Where(x => x.IsDeleted == false);

        // Include properties
        query = includeProperties.Aggregate(query, (current, includeProperty) => current.Include(includeProperty));

        // Apply orderByExpression if provided
        if (orderByExpression == null)
        {
            orderByExpression = x => x.OrderByDescending(s => s.Id);
        }
        query = orderByExpression(query);

        return await query.ToListAsync(cancellationToken);
    }


    /****************************************************************************************************************/




    // Entity All Collection With Pagination
    public IQueryable<T> Find<T>(Expression<Func<T, bool>> filterExpression,
        Func<IQueryable<T>, IOrderedQueryable<T>>? orderByExpression = null,
        int? pageNumber = null,
        int? pageSize = null,
        params string[] includeProperties) where T : TEntity
    {
        IQueryable<T> query = _context.Set<T>().Where(filterExpression);

        // Apply additional conditions for IsDeleted
        query = query.Where(x => x.IsDeleted == false);

        // Include properties
        query = includeProperties.Aggregate(query, (current, includeProperty) => current.Include(includeProperty));

        // Apply orderByExpression if provided
        if (orderByExpression == null)
        {
            orderByExpression = x => x.OrderByDescending(s => s.Id);
        }
        query = orderByExpression(query);

        // Apply pagination
        query = ApplyPagination(query, pageNumber, pageSize);

        return query;
    }

    public async Task<IEnumerable<T>> FindAsync<T>(Expression<Func<T, bool>> filterExpression,
        Func<IQueryable<T>, IOrderedQueryable<T>>? orderByExpression = null,
        int? pageNumber = null,
        int? pageSize = null,
        CancellationToken cancellationToken = default,
        params string[] includeProperties) where T : TEntity
    {
        IQueryable<T> query = _context.Set<T>().Where(filterExpression);

        // Apply additional conditions for IsDeleted
        query = query.Where(x => x.IsDeleted == false);

        // Include properties
        query = includeProperties.Aggregate(query, (current, includeProperty) => current.Include(includeProperty));

        // Apply orderByExpression if provided
        if (orderByExpression == null)
        {
            orderByExpression = x => x.OrderByDescending(s => s.Id);
        }
        query = orderByExpression(query);

        // Apply pagination
        query = ApplyPagination(query, pageNumber, pageSize);

        return await query.ToListAsync(cancellationToken);
    }



    /****************************************************************************************************************/



    // Single Entity
    public async Task<T?> GetByIdAsync<T>(long id,
        CancellationToken cancellationToken = default,
        params string[] includeProperties) where T : TEntity
    {
        IQueryable<T> query = _context.Set<T>();
        query = includeProperties.Aggregate(query, (current, includeProperty) => current.Include(includeProperty));
        return await query.AsNoTracking().FirstOrDefaultAsync(e => e.Id == id && e.IsDeleted == false, cancellationToken);
    }

    public async Task<T?> SingleOrDefaultAsync<T>(Expression<Func<T, bool>> filterExpression,
        CancellationToken cancellationToken = default,
        params string[] includeProperties) where T : TEntity
    {
        IQueryable<T> query = _context.Set<T>().Where(filterExpression);

        // Apply additional conditions for IsDeleted
        query = query.Where(x => x.IsDeleted == false);

        query = includeProperties.Aggregate(query, (current, includeProperty) => current.Include(includeProperty));
        return await query.SingleOrDefaultAsync(cancellationToken);
    }

    public async Task<T?> FirstOrDefaultAsync<T>(Expression<Func<T, bool>> filterExpression,
        CancellationToken cancellationToken = default,
        params string[] includeProperties) where T : TEntity
    {
        IQueryable<T> query = _context.Set<T>().Where(filterExpression);

        // Apply additional conditions for IsDeleted
        query = query.Where(x => x.IsDeleted == false);

        query = includeProperties.Aggregate(query, (current, includeProperty) => current.Include(includeProperty));
        return await query.FirstOrDefaultAsync(cancellationToken);
    }


    /****************************************************************************************************************/


    // Add Entity
    public void Add<T>(T entity) where T : TEntity
    {
        _context.Set<T>().Add(entity);
    }

    public async Task AddRangeAsync<T>(IEnumerable<T> entities,
        CancellationToken cancellationToken = default) where T : TEntity
    {
        await _context.Set<T>().AddRangeAsync(entities, cancellationToken);
    }


    /****************************************************************************************************************/


    // Update Entity
    public void Update<T>(T entity) where T : TEntity
    {
        if (entity.Id < 1)
            return;

        _context.Set<T>().Update(entity);
    }

    public void UpdateRange<T>(IEnumerable<T> entities) where T : TEntity
    {
        foreach (var item in entities)
        {
            if (item.Id < 1)
                return;
        }

        _context.Set<T>().UpdateRange(entities);
    }


    /****************************************************************************************************************/


    // Remove Entity
    public void Remove<T>(T entity) where T : TEntity
    {
        entity.IsDeleted = true;
        Update<T>(entity);

        //_context.Set<T>().Remove(entity);
    }

    public void RemoveRange<T>(IEnumerable<T> entities) where T : TEntity
    {
        foreach (var item in entities)
        {
            item.IsDeleted = true;
        }
        UpdateRange<T>(entities);

        //_context.Set<T>().RemoveRange(entities);
    }


    /****************************************************************************************************************/


    // Count Entity
    public async Task<int> CountAsync<T>(Expression<Func<T, bool>> filterExpression,
        CancellationToken cancellationToken = default) where T : TEntity
    {
        IQueryable<T> query = _context.Set<T>().Where(filterExpression);

        // Apply additional conditions for IsDeleted
        query = query.Where(x => x.IsDeleted == false);

        return await query.CountAsync(filterExpression, cancellationToken);
    }


    /****************************************************************************************************************/


    // Check Exists Entity
    public async Task<bool> AnyAsync<T>(Expression<Func<T, bool>> filterExpression, CancellationToken cancellationToken = default) where T : TEntity
    {
        return await _context.Set<T>().AnyAsync(filterExpression, cancellationToken);
    }


    /****************************************************************************************************************/



    // Private Apply Pagination
    private IQueryable<T> ApplyPagination<T>(IQueryable<T> query, int? pageNumber, int? pageSize) where T : TEntity
    {
        if (!pageNumber.HasValue)
        {
            pageNumber = 1;
        }
        if (!pageSize.HasValue)
        {
            pageSize = 50;
        }

        pageNumber = pageNumber == 0 ? 1 : pageNumber;
        pageSize = pageSize > 50 ? 50 : pageSize;

        int skip = (pageNumber.Value - 1) * pageSize.Value;
        return query.Skip(skip).Take(pageSize.Value);
    }
}
