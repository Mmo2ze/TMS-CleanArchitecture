namespace TMS.Contracts.Session.Create;

public record CreateSessionRequest(string GroupId,  DayOfWeek Day, DateOnly StartTime, DateOnly EndTime);