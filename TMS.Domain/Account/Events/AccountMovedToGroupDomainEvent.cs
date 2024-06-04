using TMS.Domain.Groups;

namespace TMS.Domain.Account.Events;

public record AccountMovedToGroupDomainEvent(Guid Id,Account Account, GroupId? OldGroupId, GroupId? NewGroupId) : DomainEvent(Id)
{

}