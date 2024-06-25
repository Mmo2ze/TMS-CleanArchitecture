using TMS.Domain.Students;

namespace TMS.Contracts.Attendance.Update;

public record UpdateAttendanceRequest(string Id, AttendanceStatus Status);