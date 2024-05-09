using ErrorOr;
using MediatR;
using TMS.Domain.Common.Models;

namespace TMS.Application.Groups.Queries.GetGroups;

public record GetGroupsCommand(int Page, int PageSize) : IRequest<ErrorOr<PaginatedList<GetGroupResult>>>;