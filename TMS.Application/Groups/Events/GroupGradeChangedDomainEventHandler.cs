using MediatR;
using Microsoft.EntityFrameworkCore;
using TMS.Domain.Common.Repositories;
using TMS.Domain.Groups.Events;
using TMS.Domain.Schedulers.Enums;
using TMS.Domain.Sessions;

namespace TMS.Application.Groups.Events;

public class GroupGradeChangedDomainEventHandler : INotificationHandler<GroupGradeChangedDomainEvent>
{
    private readonly ISessionRepository _sessionRepository;
    private readonly ISchedulerRepository _schedulerRepository;
    private readonly ITeacherRepository _teacherRepository;

    public GroupGradeChangedDomainEventHandler(ISessionRepository sessionRepository,
        ISchedulerRepository schedulerRepository, ITeacherRepository teacherRepository)
    {
        _sessionRepository = sessionRepository;
        _schedulerRepository = schedulerRepository;
        _teacherRepository = teacherRepository;
    }

    public async Task Handle(GroupGradeChangedDomainEvent notification, CancellationToken cancellationToken)
    {
        var schedulerOption = _teacherRepository.WhereQueryable(x => x.Id == notification.TeacherId)
            .Select(x => new { x.Id, x.AttendanceScheduler })
            .First().AttendanceScheduler;
        if (schedulerOption == AutoAttendanceSchedulerOption.AfterLastSessionOfSameGrade)
        {
            MangeOldGrades(notification);
            MangeNewGrades(notification);
        }

    }

    private void MangeNewGrades(GroupGradeChangedDomainEvent notification)
    {
        var sessions = _sessionRepository.WhereQueryable(x =>
                x.TeacherId == notification.TeacherId &&
                x.Grade == notification.NewGrade ||
                x.GroupId == notification.GroupIdId)
            .Select(x => new
            {
                x.Day,
                x.EndTime
            }).GroupBy(x => x.Day).ToList();
        var schedulers = _schedulerRepository.WhereQueryable(x =>
            x.TeacherId == notification.TeacherId &&
            x.Grade == notification.NewGrade).ToList();
        foreach (var day in sessions)
        {
            var lastSession = day.OrderByDescending(x => x.EndTime).First();
            var scheduler = schedulers.FirstOrDefault(x => x.Day == day.Key);
            if (scheduler is not null)
            {
                scheduler.UpdateFiresOn(lastSession.EndTime);
            }
            else
            {
                _schedulerRepository.Add(Domain.Schedulers.Scheduler.Create(lastSession.Day, lastSession.EndTime,
                    notification.TeacherId, notification.NewGrade));
            }
        }
        
    }

    private void MangeOldGrades(GroupGradeChangedDomainEvent notification)
    {
        var oldSessions = _sessionRepository.WhereQueryable(x =>
                x.TeacherId == notification.TeacherId &&
                x.GroupId != notification.GroupIdId &&
                x.Grade == notification.OldGrade)
            .Select(x => new
            {
                x.Day,
                x.EndTime
            }).GroupBy(x => x.Day).ToList();
        var schedulers = _schedulerRepository.WhereQueryable(x =>
                x.TeacherId == notification.TeacherId &&
                x.Grade == notification.OldGrade)
            .ToList();
        var unUsedSchedulers = schedulers
            .Where(x => oldSessions.All(y => y.Key != x.Day));
        _schedulerRepository.RemoveRange(unUsedSchedulers);
        foreach (var day in oldSessions)
        {
            var lastSession = day.OrderByDescending(x => x.EndTime).First();
            var scheduler = schedulers.FirstOrDefault(x => x.Day == day.Key);
            if (scheduler is not null)
            {
                scheduler.UpdateFiresOn(lastSession.EndTime);
            }
            else
            {
                _schedulerRepository.Add(Domain.Schedulers.Scheduler.Create(lastSession.Day, lastSession.EndTime,
                    notification.TeacherId, notification.OldGrade));
            }
        }
    }
}