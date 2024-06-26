using TMS.Domain.Groups;
using TMS.Domain.Teachers;

namespace TMS.MessagingContracts.Session;

public record SessionGradeChangedEvent(TeacherId TeacherId,TimeOnly EndTime, Grade? OldGrade, Grade? NewGrade,DayOfWeek Day);