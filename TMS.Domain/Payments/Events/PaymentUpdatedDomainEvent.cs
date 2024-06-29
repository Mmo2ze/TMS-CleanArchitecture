using TMS.Domain.Accounts;
using TMS.Domain.Teachers;

namespace TMS.Domain.Payments.Events;

public record PaymentUpdatedDomainEvent(
    PaymentId PaymentId,
    decimal Amount,
    DateOnly BillDate,
    TeacherId TeacherId,
    AccountId AccountId) : DomainEvent;