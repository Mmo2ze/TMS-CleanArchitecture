using MassTransit;
using MediatR;
using TMS.Domain.Accounts.Events;
using TMS.MessagingContracts.Payments;

namespace TMS.Application.Payments.Events;

public class PaymentCreatedDomainEventHandler : INotificationHandler<PaymentCreatedDomainEvent>
{
    private readonly IPublishEndpoint _publishEndpoint;

    public PaymentCreatedDomainEventHandler(IPublishEndpoint publishEndpoint)
    {
        _publishEndpoint = publishEndpoint;
    }

    public Task Handle(PaymentCreatedDomainEvent notification, CancellationToken cancellationToken)
    {
        return _publishEndpoint.Publish(
            new PaymentAddedEvent(notification.PaymentId, notification.AccountId!, notification.TeacherId,notification.Amount, notification.BillDate),
            cancellationToken);
    }
}