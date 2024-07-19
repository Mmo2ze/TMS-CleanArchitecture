using TMS.Domain.Attendances;
using TMS.Domain.Teachers;

namespace TMS.Domain.Accounts.Events;

public record AttendanceCreatedDomainEvent(AttendanceId AttendanceId, DateOnly AttendanceDate, AccountId AttendanceAccountId, TeacherId AttendanceTeacherId,AttendanceStatus Status) : DomainEvent;