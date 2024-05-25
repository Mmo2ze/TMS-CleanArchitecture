namespace TMS.Application.Common;

public record GetPaginatedList(int PageNumber = 1, int PageSize = 10);