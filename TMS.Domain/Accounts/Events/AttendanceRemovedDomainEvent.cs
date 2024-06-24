using TMS.Domain.Attendances;

namespace TMS.Domain.Accounts.Events;

public record AttendanceRemovedDomainEvent(AttendanceId AttendanceId, AccountId AccountId) : DomainEvent;