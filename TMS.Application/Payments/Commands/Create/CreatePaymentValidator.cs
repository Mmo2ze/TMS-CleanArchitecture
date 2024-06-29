using FluentValidation;
using TMS.Application.Common.Extensions;
using TMS.Application.Common.Services;
using TMS.Domain.Common.Errors;
using TMS.Domain.Common.Repositories;

namespace TMS.Application.Payments.Commands.Create;

public class CreatePaymentValidator : AbstractValidator<CreatePaymentCommand>
{
    private readonly ITeacherHelper _teacherHelper;
    private readonly IPaymentRepository _paymentRepository;

    public CreatePaymentValidator(ITeacherHelper teacherHelper, IPaymentRepository paymentRepository)
    {
        _teacherHelper = teacherHelper;
        _paymentRepository = paymentRepository;
        RuleFor(x => x.Amount)
            .GreaterThan(1)
            .WithMessage("Amount must be greater than 1");

        RuleFor(x => x.BillDate).MustAsync(async (command, billDate, token) =>
            {
                return !await _paymentRepository.AnyAsync(
                    x => x.AccountId == command.AccountId && x.BillDate.Year == billDate.Year &&
                         x.BillDate.Month == command.BillDate.Month, token);
            })
            .WithError(Errors.Payment.BillDateAlreadyExists);
    }
}