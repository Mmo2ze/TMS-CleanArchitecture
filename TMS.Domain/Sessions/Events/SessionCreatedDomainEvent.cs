using TMS.Domain.Groups;
using TMS.Domain.Teachers;

namespace TMS.Domain.Sessions.Events;

public record SessionCreatedDomainEvent(TeacherId TeacherId, TimeOnly EndTime, DayOfWeek Day, Grade Grade) : DomainEvent;