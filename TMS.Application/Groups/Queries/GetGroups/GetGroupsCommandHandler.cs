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
    private readonly ITeacherHelper _teacherHelper;
    private IGroupRepository _groupRepository;
    
    public GetGroupsCommandHandler(  ITeacherHelper teacherHelper, IGroupRepository groupRepository)
    {
        _teacherHelper = teacherHelper;
        _groupRepository = groupRepository;
    }

    public async Task<ErrorOr<PaginatedList<GetGroupResult>>> Handle(GetGroupsCommand request, CancellationToken cancellationToken)
    {
        var teacherId = _teacherHelper.GetTeacherId();
        if (teacherId is null)
        {
            return Errors.Auth.InvalidCredentials;
        }

        var groups =   _groupRepository.GetGroups(teacherId);
        var result = await groups
            .Select(g => GetGroupResult.FromGroup(g))
            .PaginatedListAsync(request.Page, request.PageSize);
        return result;
    }
}