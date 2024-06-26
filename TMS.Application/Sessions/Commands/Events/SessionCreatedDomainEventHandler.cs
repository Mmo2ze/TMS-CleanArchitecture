using MassTransit;
using MediatR;
using TMS.Domain.Groups.Events;
using TMS.MessagingContracts.Session;

namespace TMS.Application.Sessions.Commands.Events;

public class SessionCreatedDomainEventHandler : INotificationHandler<SessionCreatedDomainEvent>
{
    private readonly IPublishEndpoint _publishEndpoint;

    public SessionCreatedDomainEventHandler(IPublishEndpoint publishEndpoint)
    {
        _publishEndpoint = publishEndpoint;
    }

    public Task Handle(SessionCreatedDomainEvent notification, CancellationToken cancellationToken)
    {
        return _publishEndpoint.Publish(
            new SessionCreatedEvent(notification.TeacherId, notification.SessionDay, notification.SessionEndTime,
                notification.Grade), cancellationToken);
    }
}