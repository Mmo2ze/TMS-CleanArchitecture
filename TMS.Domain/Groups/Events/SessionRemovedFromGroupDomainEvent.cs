using TMS.Domain.Sessions;
using TMS.Domain.Teachers;

namespace TMS.Domain.Groups.Events;

public record SessionRemovedFromGroupDomainEvent(Guid Id, TeacherId TeacherId,SessionId SessionId,TimeOnly EndTime,DayOfWeek Day,Grade  Grade) : DomainEvent(Id);