using TMS.Domain.Payments;

namespace TMS.Domain.Accounts.Events;

public record PaymentRemovedDomainEvent(PaymentId PaymentId, AccountId AccountId) : DomainEvent;