using ErrorOr;
using MediatR;
using TMS.Domain.Accounts;

namespace TMS.Application.Payments.Commands.Create;

public record CreatePaymentCommand(decimal Amount, AccountId AccountId, DateOnly BillDate)
    : IRequest<ErrorOr<PaymentResult>>;