namespace TMS.Contracts.Session.Create;

public record SessionSummary(string GroupId, string TeacherId, DayOfWeek Day, DateOnly StartTime, DateOnly EndTime);