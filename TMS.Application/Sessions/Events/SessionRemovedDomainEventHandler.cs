using MassTransit;
using MediatR;
using TMS.Domain.Groups.Events;
using TMS.MessagingContracts.Session;

namespace TMS.Application.Sessions.Commands.Events;

public class SessionRemovedDomainEventHandler : INotificationHandler<SessionRemovedFromGroupDomainEvent>
{
    private readonly IPublishEndpoint _publishEndpoint;

    public SessionRemovedDomainEventHandler(IPublishEndpoint publishEndpoint)
    {
        _publishEndpoint = publishEndpoint;
    }

    public Task Handle(SessionRemovedFromGroupDomainEvent notification, CancellationToken cancellationToken)
    {
        return _publishEndpoint.Publish(
            new SessionRemovedEvent(notification.TeacherId, notification.Day, notification.EndTime), cancellationToken);
    }
}