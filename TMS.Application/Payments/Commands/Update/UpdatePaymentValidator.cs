using FluentValidation;
using TMS.Application.Common.Extensions;
using TMS.Application.Common.Services;
using TMS.Domain.Common.Errors;
using TMS.Domain.Common.Repositories;

namespace TMS.Application.Payments.Commands.Update;

public class UpdatePaymentValidator : AbstractValidator<UpdatePaymentCommand>
{
    private readonly ITeacherHelper _teacherHelper;

    public UpdatePaymentValidator(ITeacherHelper teacherHelper, IPaymentRepository paymentRepository)
    {
        _teacherHelper = teacherHelper;
        RuleFor(x => x.Amount)
            .GreaterThan(1)
            .WithMessage("Amount must be greater than 1");

        RuleFor(x => x.Id).MustAsync((id, token) =>
                paymentRepository.AnyAsync(x => x.TeacherId == _teacherHelper.GetTeacherId() && x.Id == id, token)
            )
            .WithError(Errors.Payment.NotFound);
    }
}