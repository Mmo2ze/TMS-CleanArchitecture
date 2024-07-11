using FluentValidation;
using TMS.Application.Common.Extensions;
using TMS.Application.Common.Services;
using TMS.Domain.Accounts;
using TMS.Domain.Common.Errors;
using TMS.Domain.Common.Repositories;

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

        RuleFor(x => x.GroupId).NotEmpty().NotNull();
        RuleFor(x => x.Id).NotEmpty()
            .MustAsync(BeFoundAccount)
            .WithError(Errors.Account.NotFound);
    }

    private Task<bool> BeFoundAccount(DeleteAccountCommand command, AccountId arg1, CancellationToken arg2)
    {
        return _accountRepository.AnyAsync(
            x => x.Id == arg1 &&
                 x.TeacherId == _teacherHelper.GetTeacherId() &&
                 x.GroupId == command.GroupId
            , arg2);
    }





}