using FluentValidation;
using TMS.Application.Common.Extensions;
using TMS.Application.Common.Services;
using TMS.Application.Common.ValidationErrors;
using TMS.Domain.Common.Repositories;
using TMS.Domain.Groups;
using TMS.Domain.Students;

namespace TMS.Application.Accounts.Commands.Create;

public class CreateAccountValidator : AbstractValidator<CreateAccountCommand>
{
    private readonly IGroupRepository _groupRepository;
    private readonly IStudentRepository _studentRepository;
    private readonly ITeacherHelper _teacherHelper;

    public CreateAccountValidator(IGroupRepository groupRepository, IStudentRepository studentRepository,
        ITeacherHelper teacherHelper)
    {
        _groupRepository = groupRepository;
        _studentRepository = studentRepository;
        _teacherHelper = teacherHelper;

        RuleFor(x => x.GroupId).NotEmpty();
        RuleFor(x => x.StudentId).NotEmpty();

        RuleFor(x => x.StudentId).MustAsync(BeFoundStudent).WithValidationError(ValidationErrors.Student.NotFound);
        RuleFor(x => x.GroupId).MustAsync(BeFoundGroup).WithValidationError(ValidationErrors.Group.NotFound);
    }

    private async Task<bool> BeFoundGroup(GroupId id, CancellationToken token)
    {
        var teacherId = _teacherHelper.GetTeacherId();
        var x =  await _groupRepository.AnyAsync(
            group => group.Id == id && group.TeacherId == teacherId, token);
        return x;
    }

    private async Task<bool> BeFoundStudent(StudentId id, CancellationToken token)
    {
        return await _studentRepository.AnyAsync(student => student.Id == id, token);
    }
}