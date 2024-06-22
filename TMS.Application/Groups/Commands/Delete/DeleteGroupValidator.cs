using FluentValidation;
using TMS.Application.Common.Extensions;
using TMS.Application.Common.Services;
using TMS.Application.Common.ValidationErrors;
using TMS.Domain.Common.Errors;
using TMS.Domain.Common.Repositories;
using TMS.Domain.Groups;

namespace TMS.Application.Groups.Commands.Delete;

public class DeleteGroupValidator:AbstractValidator<DeleteGroupCommand>
{
    private readonly IGroupRepository _groupRepository;
    private readonly ITeacherHelper _teacherHelper;
    public DeleteGroupValidator(IGroupRepository groupRepository, ITeacherHelper teacherHelper)
    {
        _groupRepository = groupRepository;
        _teacherHelper = teacherHelper;
        
        
        RuleFor(g => g.Id).MustAsync(BeFoundGroup).WithError(Errors.Group.NotFound);
    }

    private Task<bool> BeFoundGroup(GroupId arg1, CancellationToken arg2)
    {
        return _groupRepository.AnyAsync(group => group.Id == arg1 && group.TeacherId == _teacherHelper.GetTeacherId(), arg2);
    }
}