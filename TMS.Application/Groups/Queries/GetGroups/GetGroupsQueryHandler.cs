using ErrorOr;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TMS.Application.Common.Mapping;
using TMS.Application.Common.Services;
using TMS.Domain.Common.Errors;
using TMS.Domain.Common.Models;
using TMS.Domain.Common.Repositories;

namespace TMS.Application.Groups.Queries.GetGroups;

public class GetGroupsQueryHandler: IRequestHandler<GetGroupsQuery, ErrorOr<PaginatedList<GetGroupResult>>>
{
    private readonly IGroupRepository _groupRepository;
    
    public GetGroupsQueryHandler(   IGroupRepository groupRepository)
    {
        _groupRepository = groupRepository;
    }

    public async Task<ErrorOr<PaginatedList<GetGroupResult>>> Handle(GetGroupsQuery request, CancellationToken cancellationToken)
    {


        var groups =   _groupRepository.GetGroups()
            .Include(x => x.Students);
        var result = await groups
            .Select(g => GetGroupResult.FromGroup(g))
            .PaginatedListAsync(request.Page, request.PageSize);
        return result;
    }
}