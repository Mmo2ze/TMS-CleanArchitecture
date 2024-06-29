using MassTransit;
using MediatR;
using TMS.Domain.Payments.Events;
using TMS.MessagingContracts.Payments;

namespace TMS.Application.Payments.Events;

public class PaymentUpdatedDomainEventHandler: INotificationHandler<PaymentUpdatedDomainEvent>
{
    private readonly IPublishEndpoint _publishEndpoint;

    public PaymentUpdatedDomainEventHandler(IPublishEndpoint publishEndpoint)
    {
        _publishEndpoint = publishEndpoint;
    }

    public Task Handle(PaymentUpdatedDomainEvent notification, CancellationToken cancellationToken)
    {
        return _publishEndpoint.Publish(
            new PaymentUpdatedEvent(notification.PaymentId,   notification.Amount, notification.BillDate,notification.TeacherId,notification.AccountId),
            cancellationToken);
    }
}