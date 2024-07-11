using ErrorOr;
using MediatR;
using TMS.Domain.Common.Models;

namespace TMS.Application.Parents.Queries.Get;

public record GetParentsQuery(
    int Page,
    int PageSize,
    string? Search,bool PhoneRequired = true): IRequest<ErrorOr<PaginatedList<ParentResult>>>;