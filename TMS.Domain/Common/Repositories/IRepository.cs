using System.Linq.Expressions;

namespace TMS.Domain.Common.Repositories;

public interface IRepository<TEntity, TId> where TEntity : Aggregate<TId> where TId : class
{
    Task<TEntity?> GetByIdAsync(TId id);
    void Add(TEntity entity);
    void Update (TEntity entity);
    void Remove(TEntity entity);
    Task<List<TEntity>> GetAllAsync();
    IQueryable<TEntity> GetQueryable();
    bool Any(Expression<Func<TEntity, bool>> predicate);
    Task<bool> AnyAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken);
    Task<TEntity> FirstAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken);
    Task<TEntity?> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default);
}