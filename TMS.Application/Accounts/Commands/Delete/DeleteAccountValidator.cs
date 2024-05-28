using FluentValidation;
using TMS.Application.Common.Extensions;
using TMS.Application.Common.Services;
using TMS.Application.Common.ValidationErrors;
using TMS.Domain.Account;
using TMS.Domain.Common.Repositories;

namespace TMS.Application.Accounts.Commands.Delete;

public class DeleteAccountValidator: AbstractValidator<DeleteAccountCommand>
{
    private readonly IAccountRepository _accountRepository;
    private readonly ITeacherHelper _teacherHelper;

    public DeleteAccountValidator(IAccountRepository accountRepository, ITeacherHelper teacherHelper)
    {
        _accountRepository = accountRepository;
        _teacherHelper = teacherHelper;

        RuleFor(x => x.Id).MustAsync(BeFoundAccount)
            .WithValidationError(ValidationErrors.Account.NotFound);

    }
    private Task<bool> BeFoundAccount(DeleteAccountCommand command ,AccountId arg1,CancellationToken arg2)
    {
        return _accountRepository.AnyAsync(a => a.Id == arg1 && a.TeacherId == _teacherHelper.GetTeacherId() &&a.GroupId == command.GroupId, arg2);
    }
    
}