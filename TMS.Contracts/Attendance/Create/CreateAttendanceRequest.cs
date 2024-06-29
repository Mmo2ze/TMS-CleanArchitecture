using TMS.Domain.Attendances;
using TMS.Domain.Students;

namespace TMS.Contracts.Attendance.Create;

public record CreateAttendanceRequest(string AccountId, DateOnly Date,AttendanceStatus Status);

