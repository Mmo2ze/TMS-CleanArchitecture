using ErrorOr;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TMS.Application.Common.Services;
using TMS.Domain.Common.Errors;
using TMS.Domain.Common.Repositories;

namespace TMS.Application.Payments.Commands.Delete;

public class DeletePaymentCommandHandler : IRequestHandler<DeletePaymentCommand, ErrorOr<string>>
{
    private readonly ITeacherHelper _teacherHelper;
    private readonly IAccountRepository _accountRepository;
    private readonly IDateTimeProvider _dateTimeProvider;

    public DeletePaymentCommandHandler(ITeacherHelper teacherHelper, IAccountRepository accountRepository,
        IDateTimeProvider dateTimeProvider)
    {
        _teacherHelper = teacherHelper;
        _accountRepository = accountRepository;
        _dateTimeProvider = dateTimeProvider;
    }

    public async Task<ErrorOr<string>> Handle(DeletePaymentCommand request, CancellationToken cancellationToken)
    {
        var account = await _accountRepository.GetQueryable()
            .Include(x => x.Payments.Where(payment =>
                payment.Id == request.Id && payment.TeacherId == _teacherHelper.GetTeacherId()))
            .FirstOrDefaultAsync(x => x.Id == request.AccountId, cancellationToken);
        if (account is null)
            return Errors.Account.NotFound;
        var payment = account.Payments.FirstOrDefault();
        if (payment is null)
            return Errors.Payment.NotFound;
        account.RemovePayment(payment, _dateTimeProvider.Now);
        return string.Empty;
    }
}