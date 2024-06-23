using TMS.Domain.Groups;

namespace TMS.Domain.Teachers.Events;

public record GroupRemovedDomainEvent( TeacherId TeacherId, GroupId GroupId) : DomainEvent;