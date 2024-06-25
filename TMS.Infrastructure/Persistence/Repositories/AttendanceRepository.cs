using TMS.Domain.Attendances;
using TMS.Domain.Common.Repositories;

namespace TMS.Infrastructure.Persistence.Repositories;

public class AttendanceRepository: Repository<Attendance,AttendanceId>,IAttendanceRepository
{
    public AttendanceRepository(MainContext dbContext) : base(dbContext)
    {
    }
}