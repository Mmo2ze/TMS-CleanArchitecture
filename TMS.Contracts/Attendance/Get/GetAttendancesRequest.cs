namespace TMS.Contracts.Attendance.Get;

public record GetAttendancesRequest(int Page,int PageSize,string AccountId);