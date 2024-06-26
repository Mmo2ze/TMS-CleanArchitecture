using TMS.Domain.Groups;
using TMS.Domain.Teachers;

namespace TMS.Domain.Sessions.Events;

public record SessionGradeChangedDomainEvent(TeacherId TeacherId,TimeOnly SessionEndTime, Grade OldGrade,Grade NewGrade,DayOfWeek Day) : DomainEvent;