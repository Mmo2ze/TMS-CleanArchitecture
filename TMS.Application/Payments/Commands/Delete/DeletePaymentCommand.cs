using ErrorOr;
using MediatR;
using TMS.Domain.Accounts;
using TMS.Domain.Payments;

namespace TMS.Application.Payments.Commands.Delete;

public record DeletePaymentCommand(PaymentId Id,AccountId AccountId): IRequest<ErrorOr<string>>;