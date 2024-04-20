using MassTransit;
using MediatR;
using TMS.Domain.Teachers.Events;
using TMS.MessagingContracts.Teacher;

namespace TMS.Application.Teachers.Events;

public class TeacherCreatedEventHandler : INotificationHandler<TeacherCreatedDomainEvent>
{
    private readonly IPublishEndpoint _publishEndpoint;

    public TeacherCreatedEventHandler(IPublishEndpoint publishEndpoint)
    {
        _publishEndpoint = publishEndpoint;
    }

    public Task Handle(TeacherCreatedDomainEvent notification, CancellationToken cancellationToken)
    {
        _publishEndpoint.Publish(
            new TeacherCreatedEvent(notification.TeacherPhone, notification.TeacherName, notification.EndOfSubscription,
                notification.CretedByPhone), cancellationToken);
        return Task.CompletedTask;
    }
}