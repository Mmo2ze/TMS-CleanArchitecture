using MediatR;
using Microsoft.AspNetCore.Http;
using TMS.Domain.Common.Repositories;
using TMS.Domain.Teachers;
using TMS.Domain.Teachers.Events;

namespace TMS.Application.Groups.Events;

public class GroupDeletedDomainEventHandler : INotificationHandler<GroupRemovedDomainEvent>
{
    public GroupDeletedDomainEventHandler()
    {
    }

    public Task Handle(GroupRemovedDomainEvent notification, CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}