using TMS.Application.Common;

namespace TMS.Contracts.Assistant.Get;

public record GetAssistantRequest(int PageNumber, int PageSize) : GetPaginatedList(PageNumber, PageSize);