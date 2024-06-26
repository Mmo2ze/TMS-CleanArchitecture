using TMS.Domain.AttendanceSchedulers;

namespace TMS.Domain.Common.Repositories;

public interface ISchedulerRepository : IRepository<Scheduler, SchedulerId>;