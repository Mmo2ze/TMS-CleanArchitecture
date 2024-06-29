using MediatR;
using TMS.Domain.Accounts;
using TMS.Domain.Payments;
using TMS.Domain.Teachers;

namespace TMS.MessagingContracts.Payments;

public record PaymentUpdatedEvent(PaymentId Id , decimal Amount, DateOnly PaymentDate,TeacherId TeacherId,AccountId AccountId) : INotification;