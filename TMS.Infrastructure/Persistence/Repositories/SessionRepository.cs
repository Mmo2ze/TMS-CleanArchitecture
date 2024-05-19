using Microsoft.EntityFrameworkCore;
using TMS.Application.Common.Services;
using TMS.Domain.Common.Repositories;
using TMS.Domain.Groups;
using TMS.Domain.Sessions;
using TMS.Domain.Teachers;

namespace TMS.Infrastructure.Persistence.Repositories;

public class SessionRepository : Repository<Session, SessionId>, ISessionRepository
{
    private readonly TeacherId? _teacherId;

    public SessionRepository(MainContext dbContext, ITeacherHelper teacherHelper) : base(dbContext)
    {
        _teacherId = teacherHelper.GetTeacherId();
        if (_teacherId is null)
            throw new Exception("Invalid Credentials");
    }

    public Task<bool> IsValidSessionAsync(Session session, CancellationToken cancellationToken)
    {
        return IsValidSessionAsync(session.Day, session.StartTime, session.EndTime, cancellationToken);
    }

    public Task<bool> IsValidSessionAsync(DayOfWeek day, TimeOnly startTime, TimeOnly endTime,
        CancellationToken cancellationToken)
    {
        return DbContext.Sessions.AnyAsync(s => s.TeacherId == _teacherId &&
                                                s.Day == day
                                                && (
                                                    (startTime >= s.StartTime && startTime < s.EndTime)
                                                    || (endTime > s.StartTime && endTime <= s.EndTime)
                                                )
            , cancellationToken);
    }
}