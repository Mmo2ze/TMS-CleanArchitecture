using TMS.Domain.Payments;
using TMS.Domain.Teachers;

namespace TMS.Domain.Accounts.Events;

public record PaymentCreatedDomainEvent(PaymentId PaymentId, decimal Amount, DateOnly BillDate, TeacherId TeacherId,  AccountId? AccountId) : DomainEvent;