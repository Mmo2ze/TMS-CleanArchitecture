using MassTransit;
using MediatR;
using TMS.Domain.Accounts.Events;
using TMS.Domain.Common.Repositories;
using TMS.MessagingContracts.Attendances;

namespace TMS.Application.Attendance.Events;

public class AttendanceRemovedDomainEventHandler:INotificationHandler<AttendanceRemovedDomainEvent>
{
    private readonly IPublishEndpoint _publishEndpoint;
    private readonly IAttendanceRepository _attendanceRepository;

    public AttendanceRemovedDomainEventHandler(IPublishEndpoint publishEndpoint, IAttendanceRepository attendanceRepository)
    {
        _publishEndpoint = publishEndpoint;
        _attendanceRepository = attendanceRepository;
    }

    public async Task Handle(AttendanceRemovedDomainEvent notification, CancellationToken cancellationToken)
    {
      var attendance = await _attendanceRepository.FindAsync(notification.AttendanceId, cancellationToken);
      if(attendance is not null)
      {
          await _publishEndpoint.Publish(new AttendanceDeletedEvent(notification.AccountId,attendance ), cancellationToken);
          _attendanceRepository.Remove(attendance);
      }
      
    }
}