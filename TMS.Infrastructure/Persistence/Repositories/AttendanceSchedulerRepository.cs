using TMS.Domain.AttendanceSchedulers;
using TMS.Domain.Common.Repositories;

namespace TMS.Infrastructure.Persistence.Repositories;

public class AttendanceSchedulerRepository:Repository<AttendanceScheduler,AttendanceSchedulerId>, IAttendanceSchedulerRepository
{
    public AttendanceSchedulerRepository(MainContext dbContext) : base(dbContext)
    {
    }
    
}