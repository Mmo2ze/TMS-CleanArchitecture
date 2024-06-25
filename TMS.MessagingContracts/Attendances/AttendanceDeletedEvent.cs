using TMS.Domain.Accounts;
using TMS.Domain.Attendances;
using TMS.Domain.Teachers;

namespace TMS.MessagingContracts.Attendances;

public record AttendanceDeletedEvent(AccountId AccountIdId, Attendance Attendance );