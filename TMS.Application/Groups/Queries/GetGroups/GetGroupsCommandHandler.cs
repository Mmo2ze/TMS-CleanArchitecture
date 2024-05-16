using ErrorOr;
using MediatR;
using TMS.Application.Common.Mapping;
using TMS.Application.Common.Services;
using TMS.Domain.Common.Errors;
using TMS.Domain.Common.Models;
using TMS.Domain.Common.Repositories;

namespace TMS.Application.Groups.Queries.GetGroups;

public class GetGroupsCommandHandler: IRequestHandler<GetGroupsCommand, ErrorOr<PaginatedList<GetGroupResult>>>
{
    private IGroupRepository _groupRepository;
    
    public GetGroupsCommandHandler(   IGroupRepository groupRepository)
    {
        _groupRepository = groupRepository;
    }

    public async Task<ErrorOr<PaginatedList<GetGroupResult>>> Handle(GetGroupsCommand request, CancellationToken cancellationToken)
    {


        var groups =   _groupRepository.GetGroups();
        var result = await groups
            .Select(g => GetGroupResult.FromGroup(g))
            .PaginatedListAsync(request.Page, request.PageSize);
        return result;
    }
}