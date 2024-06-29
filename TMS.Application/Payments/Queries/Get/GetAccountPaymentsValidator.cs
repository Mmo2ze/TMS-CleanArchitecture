using FluentValidation;
using TMS.Application.Common.Services;
using TMS.Domain.Common.Repositories;

namespace TMS.Application.Payments.Queries.Get;

public class GetAccountPaymentsValidator : AbstractValidator<GetAccountPaymentsQuery>
{
    private readonly ITeacherHelper _teacherHelper;
    private readonly IAccountRepository _accountRepository;

    public GetAccountPaymentsValidator(ITeacherHelper teacherHelper, IAccountRepository accountRepository)
    {
        _teacherHelper = teacherHelper;
        _accountRepository = accountRepository;

        RuleFor(x => x.Id).MustAsync((id, token) =>
        {
            return _accountRepository.AnyAsync(x => x.Id == id && x.TeacherId == _teacherHelper.GetTeacherId(),
                token);
        });
    }
}