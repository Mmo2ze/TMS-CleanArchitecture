using MediatR;
using Microsoft.EntityFrameworkCore;
using TMS.Domain.Common.Repositories;
using TMS.Domain.Groups.Events;
using TMS.Domain.Schedulers.Enums;

namespace TMS.Application.Sessions.Events;

public class SessionRemovedDomainEventHandler : INotificationHandler<SessionRemovedFromGroupDomainEvent>
{
    private readonly ISchedulerRepository _schedulerRepository;
    private readonly ITeacherRepository _teacherRepository;
    private readonly ISessionRepository _sessionRepository;

    public SessionRemovedDomainEventHandler(ISchedulerRepository schedulerRepository,
        ITeacherRepository teacherRepository, ISessionRepository sessionRepository)
    {
        _schedulerRepository = schedulerRepository;
        _teacherRepository = teacherRepository;
        _sessionRepository = sessionRepository;
    }

    public async Task Handle(SessionRemovedFromGroupDomainEvent notification, CancellationToken cancellationToken)
    {
        var schedulerOption = _teacherRepository.GetQueryable()
            .Select(x => new { x.Id, x.AttendanceScheduler })
            .FirstAsync(x => x.Id == notification.TeacherId, cancellationToken)
            .Result.AttendanceScheduler;
        switch (schedulerOption)
        {
            case AutoAttendanceSchedulerOption.AfterEverySession:
            {
                await AfterEverySessionHandler(notification, cancellationToken);
                break;
            }
            case AutoAttendanceSchedulerOption.AfterLastSessionOfSameGrade:
            {
                await AfterLastSessionOfSameGradeHandler(notification, cancellationToken);
                break;
            }
        }
    }

    private async Task AfterLastSessionOfSameGradeHandler(SessionRemovedFromGroupDomainEvent notification,
        CancellationToken cancellationToken)
    {
        var scheduler = await _schedulerRepository.FirstOrDefaultAsync(x =>
                x.TeacherId == notification.TeacherId && x.Day == notification.Day && x.FiresOn == notification.EndTime,
            cancellationToken);

        if (scheduler != null)
        {
            var lastSessionOfTheDay = await _sessionRepository.WhereQueryable(x =>
                    x.TeacherId == notification.TeacherId && x.Day == notification.Day && x.Grade == notification.Grade &&x.EndTime!=notification.EndTime)
                .OrderByDescending(x => x.EndTime)
                .FirstOrDefaultAsync(cancellationToken: cancellationToken);
            if(lastSessionOfTheDay != null)
            {
               scheduler.UpdateFiresOn(lastSessionOfTheDay.EndTime);
            }
            else
            {
                _schedulerRepository.Remove(scheduler);
            }
        }
    }

    private async Task AfterEverySessionHandler(SessionRemovedFromGroupDomainEvent notification,
        CancellationToken cancellationToken)
    {
        var scheduler = await _schedulerRepository.FirstOrDefaultAsync(x =>
                x.TeacherId == notification.TeacherId && x.Day == notification.Day && x.FiresOn == notification.EndTime,
            cancellationToken);

        if (scheduler != null) _schedulerRepository.Remove(scheduler);
    }
}