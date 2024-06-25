using FluentValidation;
using TMS.Application.Common.Extensions;
using TMS.Application.Common.Services;
using TMS.Domain.Accounts;
using TMS.Domain.Common.Errors;
using TMS.Domain.Common.Repositories;

namespace TMS.Application.Attendance.Queries.Get;

public class GetAttendancesValidator : AbstractValidator<GetAttendancesQuery>
{
    private readonly ITeacherHelper _teacherHelper;
    private readonly IAccountRepository _accountRepository;

    public GetAttendancesValidator(ITeacherHelper teacherHelper, IAccountRepository accountRepository)
    {
        _teacherHelper = teacherHelper;
        _accountRepository = accountRepository;
        RuleFor(x => x.AccountId).MustAsync(BeFound)
            .WithError(Errors.Account.NotFound);
    }

    private Task<bool> BeFound(AccountId arg1, CancellationToken arg2)
    {
        return _accountRepository.AnyAsync(x => x.Id == arg1, arg2);
    }
}