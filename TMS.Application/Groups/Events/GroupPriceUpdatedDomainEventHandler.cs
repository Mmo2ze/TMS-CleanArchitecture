using MediatR;
using TMS.Domain.Common.Repositories;
using TMS.Domain.Groups.Events;

namespace TMS.Application.Groups.Events;

public class GroupPriceUpdatedDomainEventHandler:INotificationHandler<GroupPriceChangedDomainEvent>
{
    private IStudentRepository _studentRepository;

    public GroupPriceUpdatedDomainEventHandler( IStudentRepository studentRepository)
    {
        _studentRepository = studentRepository;
    }

    public Task Handle(GroupPriceChangedDomainEvent notification, CancellationToken cancellationToken)
    {
        throw new Exception();

    }
}