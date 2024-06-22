using FluentValidation;
using TMS.Application.Common.Extensions;
using TMS.Application.Common.Services;
using TMS.Application.Common.ValidationErrors;
using TMS.Domain.Account;
using TMS.Domain.Common.Errors;
using TMS.Domain.Common.Repositories;
using TMS.Domain.Groups;
using TMS.Domain.Parents;
using TMS.Domain.Students;

namespace TMS.Application.Accounts.Commands.Update;

public class UpdateAccountValidator : AbstractValidator<UpdateAccountCommand>
{
    private readonly IGroupRepository _groupRepository;
    private readonly IStudentRepository _studentRepository;
    private readonly ITeacherHelper _teacherHelper;
    private readonly IAccountRepository _accountRepository;
    private readonly IParentRepository _parentRepository;


    public UpdateAccountValidator(IGroupRepository groupRepository, IStudentRepository studentRepository,
        ITeacherHelper teacherHelper, IAccountRepository accountRepository, IParentRepository parentRepository)
    {
        _groupRepository = groupRepository;
        _studentRepository = studentRepository;
        _teacherHelper = teacherHelper;
        _accountRepository = accountRepository;
        _parentRepository = parentRepository;


        RuleFor(x => x.GroupId).NotEmpty();


        RuleFor(x => x.Id).MustAsync(BeFoundAccount).WithValidationError(Errors.Account.NotFound);
        RuleFor(x => x.StudentId).MustAsync(BeFoundStudent).WithValidationError(Errors.Student.NotFound);
        RuleFor(x => x.GroupId).MustAsync(BeFoundGroup).WithValidationError(Errors.Group.NotFound);
        RuleFor(x => x.StudentId).MustAsync(BeNotInGroup).WithValidationError(Errors.Student.AlreadyInGroup);
        RuleFor(x => x.ParentId)
            .MustAsync(BeFoundParent)
            .WithValidationError(Errors.Parnet.NotFound)
            .When(x => x.ParentId != null);
    }

    private Task<bool> BeFoundParent(ParentId arg1, CancellationToken arg2)
    {
        return _parentRepository.AnyAsync(x => x.Id == arg1, arg2);
    }


    private Task<bool> BeFoundAccount(AccountId arg1, CancellationToken arg2)
    {
        return _accountRepository.AnyAsync(a => a.Id == arg1 && a.TeacherId == _teacherHelper.GetTeacherId(), arg2);
    }

    private async Task<bool> BeNotInGroup(UpdateAccountCommand command, StudentId studentId,
        CancellationToken cancellationToken)
    {
        return !await _accountRepository.AnyAsync(
            a => a.Id != command.Id &&
                 a.StudentId == studentId &&
                 a.TeacherId == _teacherHelper.GetTeacherId(),
            cancellationToken);
    }

    private async Task<bool> BeFoundGroup(GroupId id, CancellationToken token)
    {
        var teacherId = _teacherHelper.GetTeacherId();
        var x = await _groupRepository.AnyAsync(
            group => group.Id == id && group.TeacherId == teacherId, token);
        return x;
    }

    private async Task<bool> BeFoundStudent(StudentId id, CancellationToken token)
    {
        return await _studentRepository.AnyAsync(student => student.Id == id, token);
    }
}