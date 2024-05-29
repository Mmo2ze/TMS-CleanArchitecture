namespace TMS.Contracts.Session.Get;

public record SessionResponseSummary(
    string Id,
    string GroupId,
    DayOfWeek Day,
    TimeOnly StartTime,
    TimeOnly EndTime
);