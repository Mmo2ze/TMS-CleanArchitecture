using MassTransit;
using MediatR;
using TMS.Domain.Accounts.Events;
using TMS.Domain.Attendances;
using TMS.MessagingContracts.Attendances;

namespace TMS.Application.Attendance.Events;

public class AttendanceCreatedDomainEventHandler: INotificationHandler<AttendanceCreatedDomainEvent>
{

    private readonly IPublishEndpoint _publishEndpoint;

    public AttendanceCreatedDomainEventHandler(IPublishEndpoint publishEndpoint)
    {
        _publishEndpoint = publishEndpoint;
    }

    public Task Handle(AttendanceCreatedDomainEvent notification, CancellationToken cancellationToken)
    {
        if (notification.Status == AttendanceStatus.Absent)
        {
            _publishEndpoint.Publish(
                new AbsenceRecordedEvent(notification.AttendanceId, notification.AttendanceTeacherId,
                    notification.AttendanceAccountId), cancellationToken);
        }
        return Task.CompletedTask;
    }
}