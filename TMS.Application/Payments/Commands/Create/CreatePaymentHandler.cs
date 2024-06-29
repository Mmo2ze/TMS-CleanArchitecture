using ErrorOr;
using MediatR;
using TMS.Application.Common.Services;
using TMS.Domain.Common.Errors;
using TMS.Domain.Common.Repositories;
using TMS.Domain.Payments;

namespace TMS.Application.Payments.Commands.Create;

public class CreatePaymentHandler : IRequestHandler<CreatePaymentCommand, ErrorOr<PaymentResult>>
{
    private readonly ITeacherHelper _teacherHelper;
    private readonly IAccountRepository _accountRepository;
    private readonly IDateTimeProvider _dateTimeProvider;

    public CreatePaymentHandler(IAccountRepository accountRepository, ITeacherHelper teacherHelper,
        IDateTimeProvider dateTimeProvider)
    {
        _accountRepository = accountRepository;
        _teacherHelper = teacherHelper;
        _dateTimeProvider = dateTimeProvider;
    }

    public async Task<ErrorOr<PaymentResult>> Handle(CreatePaymentCommand request, CancellationToken cancellationToken)
    {
        var account = await _accountRepository.FindAsync(request.AccountId, cancellationToken);
        if (account == null)
            return Errors.Account.NotFound;
        var teacherId = _teacherHelper.GetTeacherId();
        var assistantId = _teacherHelper.GetAssistantId();
        var payment = Payment.Create(request.Amount, _dateTimeProvider.Now, request.BillDate, teacherId, assistantId,
            request.AccountId);
        account.AddPayment(payment);
        return new PaymentResult(
            payment.Id,
            payment.Amount,
            payment.BillDate,
            _teacherHelper.GetAssistantInfo(),
            payment.CreatedAt
        );
    }
}