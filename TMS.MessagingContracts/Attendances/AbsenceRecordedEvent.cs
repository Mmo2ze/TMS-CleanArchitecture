using TMS.Domain.Accounts;
using TMS.Domain.Attendances;
using TMS.Domain.Teachers;

namespace TMS.MessagingContracts.Attendances;

public record AbsenceRecordedEvent(AttendanceId AttendanceId, TeacherId TeacherId, AccountId AccountId);