using TMS.Domain.Groups;
using TMS.Domain.Teachers;

namespace TMS.MessagingContracts.Session;

public record SessionCreatedEvent(TeacherId TeacherId,DayOfWeek Day, TimeOnly EndTime, Grade? Grade);