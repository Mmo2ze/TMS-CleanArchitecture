using TMS.Domain.Account;

namespace TMS.Domain.Groups.Events;

public record AccountRemovedFromGroupDomainEvent(Guid Id,AccountId AccountId) : DomainEvent(Id);