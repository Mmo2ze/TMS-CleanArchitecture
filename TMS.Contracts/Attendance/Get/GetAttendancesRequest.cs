namespace TMS.Contracts.Attendance.Get;

public record GetAttendancesRequest(int Month,int Year,string AccountId);