using MediatR;
using TMS.Domain.Common.Repositories;
using TMS.Domain.Schedulers.Enums;
using TMS.Domain.Sessions.Events;

namespace TMS.Application.Sessions.Events;

public class SessionCreatedDomainEventHandler : INotificationHandler<SessionCreatedDomainEvent>
{
    private readonly ITeacherRepository _teacherRepository;
    private readonly ISchedulerRepository _schedulerRepository;

    public SessionCreatedDomainEventHandler(ITeacherRepository teacherRepository,
        ISchedulerRepository schedulerRepository)
    {
        _teacherRepository = teacherRepository;
        _schedulerRepository = schedulerRepository;
    }

    public Task Handle(SessionCreatedDomainEvent notification, CancellationToken cancellationToken)
    {
        var schedulerOption = _teacherRepository.GetQueryable()
            .Select(x => new { x.Id, x.AttendanceScheduler })
            .First(x => x.Id == notification.TeacherId).AttendanceScheduler;
        switch (schedulerOption)
        {
            case AutoAttendanceSchedulerOption.AfterEverySession:
            {
                var scheduler =
                    Domain.Schedulers.Scheduler.Create(notification.Day, notification.EndTime, notification.TeacherId);
                _schedulerRepository.Add(scheduler);
                break;
            }
            case AutoAttendanceSchedulerOption.AfterLastSessionOfSameGrade:
            {
                AfterLastSessionOfSameGradeHandler(notification);
                break;
            }
        }

        return Task.CompletedTask;
    }

    private void AfterLastSessionOfSameGradeHandler(SessionCreatedDomainEvent notification)
    {
        var oldScheduler = _schedulerRepository.WhereQueryable(x =>
                x.TeacherId == notification.TeacherId && x.Grade == notification.Grade &&
                x.Day == notification.Day)
            .FirstOrDefault();
        if (oldScheduler != null)
        {
            if (oldScheduler.FiresOn < notification.EndTime)
            {
                oldScheduler.UpdateFiresOn(notification.EndTime);
            }
        }
        else
        {
            var scheduler = Domain.Schedulers.Scheduler.Create(notification.Day, notification.EndTime,
                notification.TeacherId,
                notification.Grade);
            _schedulerRepository.Add(scheduler);
        }
    }
}