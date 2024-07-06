using TMS.Contracts.Attendance.Create;

namespace TMS.Contracts.Attendance.Get;

public record GetAttendancesResponse(List<AttendanceResponse> Items,int TotalCount);