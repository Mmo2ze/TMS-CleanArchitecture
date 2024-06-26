using TMS.Domain.AttendanceSchedulers;
using TMS.Domain.Common.Repositories;

namespace TMS.Infrastructure.Persistence.Repositories;

public class SchedulerRepository:Repository<Scheduler,SchedulerId>, ISchedulerRepository
{
    public SchedulerRepository(MainContext dbContext) : base(dbContext)
    {
    }
    
}