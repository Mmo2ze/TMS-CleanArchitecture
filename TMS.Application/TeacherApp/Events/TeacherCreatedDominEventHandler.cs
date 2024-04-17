using IntegrationEvents;
using MediatR;
using TMS.Domain.Teachers.Events;

namespace TMS.Application.TeacherApp.Events;

public class TeacherCreatedDominEventHandler:INotificationHandler<TeacherCreatedDomainEvent>
{
    private readonly IPublisher _publisher;

    public TeacherCreatedDominEventHandler(IPublisher publisher)
    {
        _publisher = publisher;
    }

    public Task Handle(TeacherCreatedDomainEvent notification, CancellationToken cancellationToken)
    {
        _publisher.Publish(new TeacherCreatedIntegrationEvent(notification.Id, notification.TeacherPhone, notification.TeacherName, notification.EndOfSubscription));
        return Task.CompletedTask;
    }
}