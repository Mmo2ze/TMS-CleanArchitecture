using FluentValidation;
using TMS.Application.Common.Extensions;
using TMS.Application.Common.Services;
using TMS.Application.Common.ValidationErrors;
using TMS.Domain.Common.Repositories;
using TMS.Domain.Groups;

namespace TMS.Application.Accounts.Queries.Get;

public class GetAccountsValidator : AbstractValidator<GetAccountsQuery>
{
    private readonly ITeacherHelper _teacherHelper;
    private readonly IGroupRepository _groupRepository;

    public GetAccountsValidator(IGroupRepository groupRepository, ITeacherHelper teacherHelper)
    {
        _groupRepository = groupRepository;
        _teacherHelper = teacherHelper;

        RuleFor(x => x.Search)
            .MaximumLength(50)
            .MinimumLength(2);

        RuleFor(x => x.GroupId).MustAsync(BeFoundGroup).When(x => x.GroupId != null)
            .WithValidationError(ValidationErrors.Group.NotFound);
    }

    private async Task<bool> BeFoundGroup(GroupId? groupId, CancellationToken token)
    {
        return !await _groupRepository.AnyAsync(g => g.Id == groupId && g.TeacherId == _teacherHelper.GetTeacherId(),
            token);
    }
}