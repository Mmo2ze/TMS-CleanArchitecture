using TMS.Application.Common;

namespace TMS.Contracts.Session.Get;

public record GetSessionsRequest(string? GroupId) : GetPaginatedList;
