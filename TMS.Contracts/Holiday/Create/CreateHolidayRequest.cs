namespace TMS.Contracts.Holiday.Create;

public record CreateHolidayRequest(DateOnly StartDate, DateOnly EndDate, string? GroupId);
