using MediatR;
using TMS.Application.Common;
using TMS.Domain.Common.Models;
using TMS.Domain.Groups;
using TMS.Domain.Sessions;

namespace TMS.Application.Sessions.Queries.Get;

public record GetSessionsQuery(
    GroupId? GroupId) : GetPaginatedList, IRequest<PaginatedList<Session>>
{

};

