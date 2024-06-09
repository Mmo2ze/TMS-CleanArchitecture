using TMS.Domain.Common.Models;

namespace TMS.Contracts.Parent.Get;

public record GetParentsResponse(PaginatedList<ParentDto> Parents);