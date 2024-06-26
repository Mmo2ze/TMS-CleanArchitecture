using TMS.Domain.Teachers;

namespace TMS.Domain.Groups.Events;

public record SessionCreatedDomainEvent(TeacherId TeacherId,TimeOnly SessionEndTime, DayOfWeek SessionDay, Grade Grade) : DomainEvent;