using TMS.Domain.Teachers;

namespace TMS.MessagingContracts.Session;

public record SessionRemovedEvent(TeacherId TeacherId,DayOfWeek Day,TimeOnly EndTime);