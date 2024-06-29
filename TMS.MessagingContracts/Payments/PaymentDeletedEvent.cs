using MediatR;
using TMS.Domain.Accounts;
using TMS.Domain.Payments;
using TMS.Domain.Teachers;

namespace TMS.MessagingContracts.Payments;

public record PaymentDeletedEvent(
    PaymentId PaymentId,
    AccountId AccountId,
    TeacherId TeacherId,
    decimal Amount,
    DateOnly BillDate) : INotification;