using TMS.Domain.Accounts;
using TMS.Domain.Attendances;

namespace TMS.MessagingContracts.Attendances;

public record AttendanceDeletedEvent(AccountId AccountIdId, Attendance Attendance );