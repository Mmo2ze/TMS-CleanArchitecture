using FluentValidation;
using TMS.Application.Common.Extensions;
using TMS.Application.Common.Services;
using TMS.Application.Common.ValidationErrors;
using TMS.Domain.Account;
using TMS.Domain.Common.Repositories;
using TMS.Domain.Groups;
using TMS.Domain.Students;

namespace TMS.Application.Accounts.Commands.Update;

public class UpdateAccountPartialValidator: AbstractValidator<UpdateAccountPartialCommand>
{

    private readonly IGroupRepository _groupRepository;
    private readonly IStudentRepository _studentRepository;
    private readonly ITeacherHelper _teacherHelper;
    private readonly IAccountRepository _accountRepository;

    public UpdateAccountPartialValidator(IGroupRepository groupRepository, IStudentRepository studentRepository,
        ITeacherHelper teacherHelper, IAccountRepository accountRepository)
    {
        _groupRepository = groupRepository;
        _studentRepository = studentRepository;
        _teacherHelper = teacherHelper;
        _accountRepository = accountRepository;
        
        
        RuleFor(x => x.Id).MustAsync(BeFoundAccount)
            .WithValidationError(ValidationErrors.Account.NotFound);
        
        RuleFor(x => x.StudentId)
            .MustAsync(BeFoundStudent)
            .When(x=>x.StudentId is not null)
            .WithValidationError(ValidationErrors.Student.NotFound);
        
        RuleFor(x => x.StudentId)
            .MustAsync(BeNotInGroup)
            .When(x => x.StudentId is not null)
            .WithValidationError(ValidationErrors.Student.AlreadyInGroup);
        
        RuleFor(x => x.GroupId)
            .MustAsync(BeFoundGroup)
            .When(x => x.GroupId is not null)
            .WithValidationError(ValidationErrors.Group.NotFound);
        


    }
    
    private Task<bool> BeFoundAccount(AccountId arg1, CancellationToken arg2)
    {
        return _accountRepository.AnyAsync(a => a.Id == arg1 && a.TeacherId == _teacherHelper.GetTeacherId(), arg2);
    }

    private async Task<bool> BeNotInGroup(UpdateAccountPartialCommand command, StudentId? studentId,
        CancellationToken cancellationToken)
    {
        return !await _accountRepository.AnyAsync(
            a => a.Id != command.Id &&
                 a.StudentId == studentId &&
                 a.TeacherId == _teacherHelper.GetTeacherId(),
            cancellationToken);
    }

    private async Task<bool> BeFoundGroup(GroupId? id, CancellationToken token)
    {
        var teacherId = _teacherHelper.GetTeacherId();
        var x = await _groupRepository.AnyAsync(
            group => group.Id == id && group.TeacherId == teacherId, token);
        return x;
    }

    private async Task<bool> BeFoundStudent(StudentId? id, CancellationToken token)
    {
        return await _studentRepository.AnyAsync(student => student.Id == id, token);
    }
}