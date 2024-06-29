using ErrorOr;
using MediatR;
using TMS.Application.Payments.Commands.Create;
using TMS.Domain.Payments;

namespace TMS.Application.Payments.Commands.Update;

public record UpdatePaymentCommand(PaymentId Id, decimal Amount, DateOnly BillDate): IRequest<ErrorOr<PaymentResult>>;