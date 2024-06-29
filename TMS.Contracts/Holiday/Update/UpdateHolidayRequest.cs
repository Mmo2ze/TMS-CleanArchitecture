namespace TMS.Contracts.Holiday.Update;

public record UpdateHolidayRequest(string Id, DateOnly StartDate, DateOnly EndDate, string? GroupId);