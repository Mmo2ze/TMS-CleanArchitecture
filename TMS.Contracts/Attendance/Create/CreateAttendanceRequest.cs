using TMS.Domain.Attendances;

namespace TMS.Contracts.Attendance.Create;

public record CreateAttendanceRequest(string AccountId, DateOnly Date,AttendanceStatus Status);

