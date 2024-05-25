namespace TMS.Contracts.Session.Create;

public record CreateSessionRequest(string GroupId,  DayOfWeek Day, TimeOnly StartTime, TimeOnly EndTime);