using TMS.Domain.Common.Repositories;

namespace TMS.Infrastructure.Persistence.Repositories;

public class UnitOfWork : IUnitOfWork
{
    private readonly MainContext _dbContext;

    public UnitOfWork(MainContext dbContext)
    {
        _dbContext = dbContext;
    }

    public Task SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            return _dbContext.SaveChangesAsync(cancellationToken);
        }
        catch (Exception e)
        {
            throw new Exception(e.Message);
        }
    }
}