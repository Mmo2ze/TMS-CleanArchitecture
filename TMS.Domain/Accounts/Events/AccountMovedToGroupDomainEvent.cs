using TMS.Domain.Groups;

namespace TMS.Domain.Accounts.Events;

public record AccountMovedToGroupDomainEvent(Guid Id, Account Account, GroupId? OldGroupId, GroupId? NewGroupId) : DomainEvent(Id);