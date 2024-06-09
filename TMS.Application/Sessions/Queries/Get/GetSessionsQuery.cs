using ErrorOr;
using MediatR;
using TMS.Application.Common;
using TMS.Domain.Common.Models;
using TMS.Domain.Groups;
using TMS.Domain.Sessions;

namespace TMS.Application.Sessions.Queries.Get;

public record GetSessionsQuery(
    GroupId? GroupId,int PageNumber = 1, int PageSize = 10) : 
    IRequest<ErrorOr<PaginatedList<Session>>>;