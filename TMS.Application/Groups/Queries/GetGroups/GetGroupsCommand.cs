using ErrorOr;
using MediatR;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TMS.Domain.Common.Models;

namespace TMS.Application.Groups.Queries.GetGroups;

public record GetGroupsCommand(int Page, int PageSize) : IRequest<ErrorOr<PaginatedList<GetGroupResult>>>;