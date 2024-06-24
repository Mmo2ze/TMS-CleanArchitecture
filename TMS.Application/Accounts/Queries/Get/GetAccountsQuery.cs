using ErrorOr;
using MediatR;
using TMS.Application.Common;
using TMS.Domain.Accounts;
using TMS.Domain.Common.Models;
using TMS.Domain.Groups;

namespace TMS.Application.Accounts.Queries.Get;

public record GetAccountsQuery(
    string? Search,
    GroupId? GroupId,
    int PageNumber = 1,
    int PageSize = 10
) : GetPaginatedList(PageNumber, PageSize),
    IRequest<ErrorOr<PaginatedList<AccountSummary>>>;