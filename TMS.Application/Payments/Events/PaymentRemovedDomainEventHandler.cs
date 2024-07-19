using MassTransit;
using MediatR;
using TMS.Domain.Accounts.Events;
using TMS.Domain.Common.Repositories;
using TMS.MessagingContracts.Payments;

namespace TMS.Application.Payments.Events;

public class PaymentRemovedDomainEventHandler : INotificationHandler<PaymentRemovedDomainEvent>
{
    private readonly IPaymentRepository _paymentRepository;
    private readonly IPublishEndpoint _publishEndpoint;

    public PaymentRemovedDomainEventHandler(IPaymentRepository paymentRepository, IPublishEndpoint publishEndpoint)
    {
        _paymentRepository = paymentRepository;
        _publishEndpoint = publishEndpoint;
    }

    public async Task Handle(PaymentRemovedDomainEvent notification, CancellationToken cancellationToken)
    {
        var payment = await _paymentRepository.FindAsync(notification.PaymentId, cancellationToken);
        if (payment is not null)
        {
            _paymentRepository.Remove(payment);
            await _publishEndpoint.Publish(new PaymentDeletedEvent(payment.Id, notification.AccountId, payment.TeacherId,payment.Amount,payment.BillDate), cancellationToken);
        }
    }
}