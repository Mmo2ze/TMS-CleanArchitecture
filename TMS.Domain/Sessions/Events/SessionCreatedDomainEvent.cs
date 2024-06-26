using TMS.Domain.Teachers;

namespace TMS.Domain.Groups.Events;

internal record SessionCreatedDomainEvent(TeacherId TeacherId, TimeOnly EndTime, DayOfWeek Day, Grade Grade) : DomainEvent;