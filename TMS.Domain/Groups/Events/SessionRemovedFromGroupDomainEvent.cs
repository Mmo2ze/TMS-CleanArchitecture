using TMS.Domain.Sessions;

namespace TMS.Domain.Groups.Events;

public record SessionRemovedFromGroupDomainEvent(Guid Id, SessionId SessionId) : DomainEvent(Id)
{
}