using FluentValidation;
using TMS.Application.Common.Extensions;
using TMS.Application.Common.Services;
using TMS.Domain.Common.Constrains;
using TMS.Domain.Common.Errors;
using TMS.Domain.Common.Repositories;
using TMS.Domain.Groups;

namespace TMS.Application.Groups.Commands.Update;

public class UpdateGroupValidator : AbstractValidator<UpdateGroupCommand>
{
    private readonly IGroupRepository _groupRepository;
    private readonly ITeacherHelper _teacherHelper;

    public UpdateGroupValidator(IGroupRepository groupRepository, ITeacherHelper teacherHelper)
    {
        _groupRepository = groupRepository;
        _teacherHelper = teacherHelper;
        RuleFor(x => x.Name)
            .Length(Constrains.Group.Name);

        RuleFor(x => x.Grade)
            .IsInEnum();

        RuleFor(x => x.BasePrice)
            .GreaterThanOrEqualTo(1);
        RuleFor(x => x.Id)
            .MustAsync(BeFound).WithError(Errors.Group.NotFound);
        RuleFor(x => x.Name).MustAsync(BeUnique)
            .WithError(Errors.Group.NameAlreadyExists);
    }

    private async Task<bool> BeUnique(UpdateGroupCommand command, string name, CancellationToken token)
    {
        var teacherId = _teacherHelper.GetTeacherId();
        return !await _groupRepository.AnyAsync(
            group => group.Name == name &&
                     group.TeacherId == teacherId &&
                     group.Grade == command.Grade &&
                     group.Id != command.Id
            , token);
    }

    private Task<bool> BeFound(GroupId id, CancellationToken cancellationToken)
    {
        var teacherId = _teacherHelper.GetTeacherId();
        return _groupRepository.AnyAsync(group => group.Id == id && group.TeacherId == teacherId, cancellationToken);
    }
}