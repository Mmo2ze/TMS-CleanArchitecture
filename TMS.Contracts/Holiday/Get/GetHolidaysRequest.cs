namespace TMS.Contracts.Holiday.Get;

public record GetHolidaysRequest(int Page, int PageSize, string? GroupId);