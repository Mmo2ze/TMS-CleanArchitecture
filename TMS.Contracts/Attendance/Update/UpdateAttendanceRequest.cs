using TMS.Domain.Attendances;

namespace TMS.Contracts.Attendance.Update;

public record UpdateAttendanceRequest(string Id, AttendanceStatus Status);