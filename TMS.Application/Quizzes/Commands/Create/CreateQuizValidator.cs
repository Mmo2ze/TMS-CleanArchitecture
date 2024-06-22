using FluentValidation;
using TMS.Application.Common.Extensions;
using TMS.Application.Common.Services;
using TMS.Application.Common.ValidationErrors;
using TMS.Domain.Account;
using TMS.Domain.Common.Errors;
using TMS.Domain.Common.Repositories;

namespace TMS.Application.Quizzes.Commands.Create;

public class CreateQuizValidator : AbstractValidator<CreateQuizCommand>
{
    private readonly IAccountRepository _accountRepository;
    private readonly ITeacherHelper _teacherHelper;
    private readonly IDateTimeProvider _dateTimeProvider;

    public CreateQuizValidator(ITeacherHelper teacherHelper, IAccountRepository accountRepository, IDateTimeProvider dateTimeProvider)
    {
        _teacherHelper = teacherHelper;
        _accountRepository = accountRepository;
        _dateTimeProvider = dateTimeProvider;


        RuleFor(x => x.Degree)
            .GreaterThanOrEqualTo(0)
            .WithMessage("Degree must be greater than or equal to 0")
            .LessThanOrEqualTo(x => x.MaxDegree)
            .WithMessage("Degree must be less than or equal to MaxDegree");
        RuleFor(x => x.AccountId)
            .MustAsync(AccountExists)
            .WithValidationError(Errors.Account.NotFound);

        RuleFor(x => x.AccountId)
            .MustAsync(AccountDoseNotHaveQuizToday)
            .WithValidationError(Errors.Account.HasQuizToday);
    }

    private async Task<bool> AccountDoseNotHaveQuizToday(AccountId arg1, CancellationToken arg2)
    {
        return !await _accountRepository.AnyAsync(
            x => x.Id == arg1 && x.Quizzes.Any(x => x.CreatedAt == _dateTimeProvider.Today )
            , arg2);
    }

    private Task<bool> AccountExists(AccountId arg1, CancellationToken arg2)
    {
        return _accountRepository.AnyAsync(
            x => x.Id == arg1 && x.TeacherId == _teacherHelper.GetTeacherId()
            , arg2);
    }
}