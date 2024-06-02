using FluentValidation;
using TMS.Application.Common.Services;
using TMS.Domain.Common.Repositories;
using TMS.Domain.Groups;
using TMS.Domain.Students;

namespace TMS.Application.Accounts.Commands.Delete;

public class CreatAccountValidator : AbstractValidator<DeleteAccountCommand>
{
    private readonly IGroupRepository _groupRepository;
    private readonly IStudentRepository _studentRepository;
    private readonly ITeacherHelper _teacherHelper;
    private readonly IAccountRepository _accountRepository;

    public CreatAccountValidator(IGroupRepository groupRepository, IStudentRepository studentRepository,
        ITeacherHelper teacherHelper, IAccountRepository accountRepository)
    {
        _groupRepository = groupRepository;
        _studentRepository = studentRepository;
        _teacherHelper = teacherHelper;
        _accountRepository = accountRepository;

        Console.Error.WriteLine("DeleteAccountCommandValidator");
        RuleFor(x => x.GroupId).NotEmpty();
       
    }

    private async Task<bool> BeNotInGroup(StudentId studentId, CancellationToken cancellationToken)
    {
        return !await _accountRepository.AnyAsync(
            a => a.StudentId == studentId && a.TeacherId == _teacherHelper.GetTeacherId(), cancellationToken);
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