using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using TMS.Domain.Common.Models;
using TMS.Domain.Common.Repositories;

namespace TMS.Infrastructure.Persistence.Repositories;

public abstract class Repository<TEntity, TId> : IRepository<TEntity, TId> where TEntity : Aggregate<TId>
    where TId : class

{
    protected readonly MainContext DbContext;

    protected Repository(MainContext dbContext)
    {
        DbContext = dbContext;
    }

    public Task<TEntity?> GetByIdAsync(TId id, CancellationToken cancellationToken = default)
    {
        return DbContext.Set<TEntity>().FirstOrDefaultAsync(e => e.Id == id, cancellationToken: cancellationToken);
    }

    public void Add(TEntity entity)
    {
        DbContext.Add(entity);
    }

    public void Update(TEntity entity)
    {
        DbContext.Update(entity);
    }

    public void Remove(TEntity entity)
    {
        DbContext.Remove(entity);
    }

    public void RemoveRange(IEnumerable<TEntity> entities)
    {
        DbContext.RemoveRange(entities);
    }

    public async Task<List<TEntity>> GetAllAsync()
    {
        return await DbContext.Set<TEntity>().ToListAsync();
    }

    public IQueryable<TEntity> GetQueryable()
    {
        return DbContext.Set<TEntity>();
    }

    public Task<bool> AnyAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken)
    {
        return DbContext.Set<TEntity>().AnyAsync(predicate, cancellationToken);
    }

    public Task<TEntity> FirstAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken)
    {
        return DbContext.Set<TEntity>().FirstAsync(predicate, cancellationToken);
    }

    public Task<TEntity?> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate,
        CancellationToken cancellationToken = default)
    {
        return DbContext.Set<TEntity>().FirstOrDefaultAsync(predicate, cancellationToken);
    }

    public Task<TEntity?> FindAsync(TId id, CancellationToken cancellationToken = default)
    {
        return DbContext.Set<TEntity>().FirstOrDefaultAsync(e => e.Id == id, cancellationToken);
    }

    public Task<bool> AllAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken)
    {
        return DbContext.Set<TEntity>().AllAsync(predicate, cancellationToken);
    }

    public Task<List<TEntity>> Where(Expression<Func<TEntity, bool>> predicate)
    {
        return DbContext.Set<TEntity>().Where(predicate).ToListAsync();
    }

    public IQueryable<TEntity> WhereQueryable(Expression<Func<TEntity, bool>> predicate)
    {
        return DbContext.Set<TEntity>().Where(predicate);  
    }


    public bool Any(Expression<Func<TEntity, bool>> predicate)
    {
        return DbContext.Set<TEntity>().Any(predicate);
    }
}