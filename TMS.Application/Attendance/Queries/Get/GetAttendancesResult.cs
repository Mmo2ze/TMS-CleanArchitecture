using TMS.Application.Attendance.Commands.Create;

namespace TMS.Application.Attendance.Queries.Get;

public record GetAttendancesResult(List<AttendanceResult> Items, int TotalCount);