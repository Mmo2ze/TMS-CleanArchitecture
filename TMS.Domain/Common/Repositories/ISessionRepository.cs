using TMS.Domain.Groups;
using TMS.Domain.Sessions;

namespace TMS.Domain.Common.Repositories;

public interface ISessionRepository: IRepository<Session,SessionId>
{
    Task<bool> IsValidSessionAsync(Session session, CancellationToken cancellationToken);
    Task<bool> IsValidSessionAsync(DayOfWeek day, TimeOnly startTime, TimeOnly endTime, CancellationToken cancellationToken);

}