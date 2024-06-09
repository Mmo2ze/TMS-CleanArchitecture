using ErrorOr;
using MediatR;
using TMS.Application.Common;
using TMS.Domain.Common.Models;

namespace TMS.Application.Parents.Queries.Get;

public record GetParentsQuery(
    int PageNumber,
    int PageSize,
    string? Search): IRequest<ErrorOr<PaginatedList<ParentResult>>>;