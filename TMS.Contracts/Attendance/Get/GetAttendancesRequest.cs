namespace TMS.Contracts.Attendance;

public record GetAttendancesRequest(int Page,int PageSize,string AccountId);