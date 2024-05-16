using MassTransit;
using MediatR;
using TMS.Domain.Teachers;
using TMS.MessagingContracts.Teacher;

namespace TMS.Application.Teachers.Events;

public class SubscriptionAddedEventHandler : INotificationHandler<SubscriptionAddedDomainEvent>
{
    private readonly IBus _publishEndpoint;

    public SubscriptionAddedEventHandler(IBus publishEndpoint)
    {
        _publishEndpoint = publishEndpoint;
    }

    public Task Handle(SubscriptionAddedDomainEvent notification, CancellationToken cancellationToken)
    {
        PublishEvent(notification, cancellationToken);

        return Task.CompletedTask;
    }

    private void PublishEvent(SubscriptionAddedDomainEvent notification, CancellationToken cancellationToken)
    {
        _publishEndpoint.Publish(notification, cancellationToken);
    }
}