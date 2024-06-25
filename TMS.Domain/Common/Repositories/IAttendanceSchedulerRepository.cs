using TMS.Domain.AttendanceSchedulers;

namespace TMS.Domain.Common.Repositories;

public interface IAttendanceSchedulerRepository : IRepository<AttendanceScheduler, AttendanceSchedulerId>;