using FluentValidation;
using TMS.Application.Common.Extensions;
using TMS.Application.Common.Services;
using TMS.Domain.Common.Constrains;
using TMS.Domain.Common.Errors;
using TMS.Domain.Common.Repositories;

namespace TMS.Application.Groups.Commands.Create;

public class CreateGroupValidator : AbstractValidator<CreateGroupCommand>
{
    private readonly IGroupRepository _groupRepository;
    private readonly ITeacherHelper _teacherHelper;

    public CreateGroupValidator(IGroupRepository groupRepository, ITeacherHelper teacherHelper)
    {
        _groupRepository = groupRepository;
        _teacherHelper = teacherHelper;

        RuleFor(x => x.Name)
            .Length(Constrains.Group.Name);
        RuleFor(x => x.Name).MustAsync(BeNotUsed)
            .WithError(Errors.Group.GroupNameAlreadyExists);
        RuleFor(x => x.BasePrice).GreaterThan(1);
    }

    private async Task<bool> BeNotUsed(CreateGroupCommand command, string arg1, CancellationToken arg2)
    {
        return !await _groupRepository.AnyAsync(
            x => x.Name == arg1 && x.TeacherId == _teacherHelper.GetTeacherId() && x.Grade == command.Grade,
            arg2);
    }
}