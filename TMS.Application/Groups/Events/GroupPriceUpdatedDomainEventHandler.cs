using MediatR;
using TMS.Domain.Common.Repositories;
using TMS.Domain.Groups.Events;

namespace TMS.Application.Groups.Events;

public class GroupPriceUpdatedDomainEventHandler : INotificationHandler<GroupPriceChangedDomainEvent>
{
    private IStudentRepository _studentRepository;

    public GroupPriceUpdatedDomainEventHandler(IStudentRepository studentRepository)
    {
        _studentRepository = studentRepository;
    }

    public async Task Handle(GroupPriceChangedDomainEvent notification, CancellationToken cancellationToken)
    {
        
            //var students =  _studentRepository.GetByGroupIdQuery(notification.GroupId);

    }
}