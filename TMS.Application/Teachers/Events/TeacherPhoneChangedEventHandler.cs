using MassTransit;
using MediatR;
using TMS.Domain.Teachers.Events;
using TMS.MessagingContracts.Teacher;

namespace TMS.Application.Teachers.Events;

public class TeacherPhoneChangedEventHandler : INotificationHandler<TeacherPhoneChangedDoaminEvent>
{
    private readonly IPublishEndpoint _publishEndpoint;

    public TeacherPhoneChangedEventHandler(IPublishEndpoint publishEndpoint )
    {
        _publishEndpoint = publishEndpoint;
    }


    public Task Handle(TeacherPhoneChangedDoaminEvent notification, CancellationToken cancellationToken)
    {
        _publishEndpoint.Publish(
            new TeacherPhoneChangedEvent(notification.Phone,notification.TeacherId), cancellationToken);  
        return Task.CompletedTask;
    }
}