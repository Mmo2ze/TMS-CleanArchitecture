using System.Globalization;
using FluentValidation;
using TMS.Application.Common.Extensions;
using TMS.Application.Common.Services;
using TMS.Application.Common.ValidationErrors;
using TMS.Domain.Account;
using TMS.Domain.Common.Repositories;

namespace TMS.Application.Quizzes.Queries.Get;

public class GetQuizzesValidator : AbstractValidator<GetQuizzesQuery>
{
    private readonly ITeacherHelper _teacherHelper;
    private readonly IAccountRepository _accountRepository;

    public GetQuizzesValidator(IAccountRepository accountRepository, ITeacherHelper teacherHelper)
    {
        _accountRepository = accountRepository;
        _teacherHelper = teacherHelper;
        RuleFor(x => x.AccountId)
            .MustAsync(BeFoundAccount)
            .WithValidationError(ValidationErrors.Account.NotFound);
    }

    private Task<bool> BeFoundAccount(AccountId arg1, CancellationToken arg2)
    {
        return _accountRepository.AnyAsync(x => x.Id == arg1 && x.TeacherId == _teacherHelper.GetTeacherId(), arg2);
    }
}