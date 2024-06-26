using MediatR;
using TMS.Domain.Common.Repositories;
using TMS.Domain.Groups;

namespace TMS.Application.Groups.Events;

public class GroupGradeChangedDomainEventHandler : INotificationHandler<GroupGradeChangedDomainEvent>
{
    private readonly ISessionRepository _sessionRepository;
    private readonly ISchedulerRepository _schedulerRepository;

    public GroupGradeChangedDomainEventHandler(ISessionRepository sessionRepository,
        ISchedulerRepository schedulerRepository)
    {
        _sessionRepository = sessionRepository;
        _schedulerRepository = schedulerRepository;
    }

    public  Task Handle(GroupGradeChangedDomainEvent notification, CancellationToken cancellationToken)
    {
        var sessions = _sessionRepository.WhereQueryable(x => x.TeacherId == notification.TeacherId)
            .Select(x => new { x.EndTime, x, x.Day })
            .ToList();

        var days = sessions.Select(x => x.Day).Distinct();
        var endTimes = sessions.Select(x => x.EndTime).Distinct();
        var schedulers = _schedulerRepository.WhereQueryable(x =>
                x.TeacherId == notification.TeacherId &&
                x.Grade == notification.OldGrade
                && days.Contains(x.Day) &&
                endTimes.Contains(x.FiresOn)
            )
            .ToList();
        foreach (var session in sessions)
        {
            var scheduler = schedulers.FirstOrDefault(x => x.Day == session.Day && x.FiresOn == session.EndTime);
            scheduler?.UpdateGrade(notification.NewGrade);
        }
        return Task.CompletedTask;
    }
}