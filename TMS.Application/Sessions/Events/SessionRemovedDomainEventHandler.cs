using MediatR;
using TMS.Domain.Common.Repositories;
using TMS.Domain.Groups.Events;

namespace TMS.Application.Sessions.Events;

public class SessionRemovedDomainEventHandler : INotificationHandler<SessionRemovedFromGroupDomainEvent>
{
    private readonly ISchedulerRepository _schedulerRepository;

    public SessionRemovedDomainEventHandler(ISchedulerRepository schedulerRepository)
    {
        _schedulerRepository = schedulerRepository;
    }

    public async Task Handle(SessionRemovedFromGroupDomainEvent notification, CancellationToken cancellationToken)
    {
        var scheduler = await _schedulerRepository.FirstOrDefaultAsync(x =>
                x.TeacherId == notification.TeacherId && x.Day == notification.Day && x.FiresOn == notification.EndTime,
            cancellationToken);
        if (scheduler != null) _schedulerRepository.Remove(scheduler);
    }
}