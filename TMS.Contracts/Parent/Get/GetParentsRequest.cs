using TMS.Application.Common;

namespace TMS.Contracts.Parent.Get;

public record GetParentsRequest(
    int PageNumber,
    int PageSize,
    string? Search) ;