using TMS.Domain.Common.Models;
using TMS.Domain.Groups;

namespace TMS.Contracts.Group.GetGroups;

public record GetGroupsResponse(PaginatedList<GetGroupResponse> Groups );

public record GetGroupResponse(string GroupId, string Name, Grade Grade, double BasePrice, int StudentsCount, int SessionsCount);