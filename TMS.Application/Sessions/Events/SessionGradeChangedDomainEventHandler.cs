using System.Net;
using MassTransit;
using MediatR;
using TMS.Domain.Sessions.Events;
using TMS.MessagingContracts.Session;

namespace TMS.Application.Sessions.Events;

public class SessionGradeChangedDomainEventHandler : INotificationHandler<SessionGradeChangedDomainEvent>
{
    private readonly IPublishEndpoint _publishEndpoint;

    public SessionGradeChangedDomainEventHandler(IPublishEndpoint publishEndpoint)
    {
        _publishEndpoint = publishEndpoint;
    }

    public Task Handle(SessionGradeChangedDomainEvent notification, CancellationToken cancellationToken)
    {
        return _publishEndpoint.Publish(
            new SessionGradeChangedEvent(
                notification.TeacherId,
                notification.SessionEndTime,
                notification.OldGrade,
                notification.NewGrade,
                notification.Day), cancellationToken);
    }
}