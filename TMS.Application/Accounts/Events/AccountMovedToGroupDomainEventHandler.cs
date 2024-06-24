using MediatR;
using TMS.Domain.Accounts.Events;
using TMS.Domain.Common.Repositories;
using TMS.Domain.Groups;

namespace TMS.Application.Accounts.Events;

public class AccountMovedToGroupDomainEventHandler : INotificationHandler<AccountMovedToGroupDomainEvent>
{
    private readonly IGroupRepository _groupRepository;
    private readonly IUnitOfWork _unitOfWork;

    public AccountMovedToGroupDomainEventHandler(IGroupRepository groupRepository, IUnitOfWork unitOfWork)
    {
        _groupRepository = groupRepository;
        _unitOfWork = unitOfWork;
    }

    public Task Handle(AccountMovedToGroupDomainEvent notification, CancellationToken cancellationToken)
    {
        List<GroupId> ids = [notification.OldGroupId!, notification.NewGroupId!];
        var groups = _groupRepository.GetQueryable().Where(x => ids.Contains(x.Id)).ToList();
        var oldGroup = groups.FirstOrDefault(x => x.Id == notification.OldGroupId);
        var newGroup = groups.FirstOrDefault(x => x.Id == notification.NewGroupId);

        oldGroup?.RemoveStudent(notification.Account);
        newGroup?.AddStudent(notification.Account);
        return _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}