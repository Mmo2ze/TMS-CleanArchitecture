using TMS.Domain.Common.Repositories;
using TMS.Domain.Schedulers;

namespace TMS.Infrastructure.Persistence.Repositories;

public class SchedulerRepository:Repository<Scheduler,SchedulerId>, ISchedulerRepository
{
    public SchedulerRepository(MainContext dbContext) : base(dbContext)
    {
    }
    
}