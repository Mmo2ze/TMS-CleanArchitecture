using FluentValidation;
using TMS.Application.Accounts.Queries.Get;
using TMS.Application.Common.Extensions;
using TMS.Application.Common.Services;
using TMS.Application.Common.ValidationErrors;
using TMS.Domain.Common.Errors;
using TMS.Domain.Common.Repositories;
using TMS.Domain.Groups;

namespace TMS.Application.Sessions.Queries.Get;

public class GetSessionsValidator : AbstractValidator<GetSessionsQuery>
{
    private readonly ITeacherHelper _teacherHelper;
    private readonly IGroupRepository _groupRepository;

    public GetSessionsValidator(IGroupRepository groupRepository, ITeacherHelper teacherHelper)
    {
        _groupRepository = groupRepository;
        _teacherHelper = teacherHelper;

       

        RuleFor(x => x.GroupId).MustAsync(BeFoundGroup).When(x => x.GroupId != null)
            .WithError(Errors.Group.NotFound);
    }

    private async Task<bool> BeFoundGroup(GroupId? groupId, CancellationToken token)
    {
        return await _groupRepository.AnyAsync(g => g.Id == groupId && g.TeacherId == _teacherHelper.GetTeacherId(),
            token);
    }
}