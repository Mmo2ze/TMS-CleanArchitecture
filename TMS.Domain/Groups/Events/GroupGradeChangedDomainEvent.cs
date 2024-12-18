using TMS.Domain.Teachers;

namespace TMS.Domain.Groups.Events;

public record GroupGradeChangedDomainEvent(TeacherId TeacherId,GroupId GroupIdId, Grade OldGrade, Grade? NewGrade) : DomainEvent;