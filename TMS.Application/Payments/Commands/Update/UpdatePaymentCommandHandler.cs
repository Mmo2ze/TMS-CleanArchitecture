using ErrorOr;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TMS.Application.Common.Services;
using TMS.Application.Payments.Commands.Create;
using TMS.Domain.Accounts;
using TMS.Domain.Assistants;
using TMS.Domain.Common.Errors;
using TMS.Domain.Common.Repositories;

namespace TMS.Application.Payments.Commands.Update;

public class UpdatePaymentCommandHandler : IRequestHandler<UpdatePaymentCommand, ErrorOr<PaymentResult>>
{
    private readonly ITeacherHelper _teacherHelper;
    private readonly IDateTimeProvider _dateTimeProvider;
    private readonly IPaymentRepository _paymentRepository;

    public UpdatePaymentCommandHandler(ITeacherHelper teacherHelper, IDateTimeProvider dateTimeProvider,
        IPaymentRepository paymentRepository)
    {
        _teacherHelper = teacherHelper;
        _dateTimeProvider = dateTimeProvider;
        _paymentRepository = paymentRepository;
    }

    public async Task<ErrorOr<PaymentResult>> Handle(UpdatePaymentCommand request, CancellationToken cancellationToken)
    {
        var payment = await _paymentRepository.GetQueryable()
            .Include(x => x.CreatedBy)
            .Include(x => x.ModifiedBy)
            .FirstAsync(x => x.Id == request.Id, cancellationToken);

        if (payment.AccountId is null)
            return Errors.Payment.AccountHasBeenDeleted;

        if (! await IsValidDate(request, payment.AccountId, cancellationToken))
            return Errors.Payment.BillDateAlreadyExists;

        payment.Update(request.Amount, request.BillDate, _dateTimeProvider.Now, _teacherHelper.GetAssistantId());

        return new PaymentResult(
            payment.Id,
            payment.Amount,
            payment.BillDate,
            payment.CreatedBy != null
                ? new AssistantInfo(payment.CreatedBy.Name, payment.CreatedById)
                : _teacherHelper.TeacherInfo(),
            payment.CreatedAt,
            payment.ModifiedBy != null
                ? new AssistantInfo(payment.ModifiedBy.Name, payment.CreatedById)
                : _teacherHelper.TeacherInfo(),
            payment.ModifiedAt
        );
    }

    private async Task<bool> IsValidDate(UpdatePaymentCommand command, AccountId accountId,
        CancellationToken cancellationToken)
    {
        return !await _paymentRepository.WhereQueryable(x => x.AccountId == accountId && x.Id != command.Id).AnyAsync(
            x => x.BillDate.Year == command.BillDate.Year &&
                 x.BillDate.Month == command.BillDate.Month
            , cancellationToken);
    }
}