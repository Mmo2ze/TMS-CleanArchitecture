namespace TMS.Domain.Groups.Events;

public record GroupPriceChangedDomainEvent(Guid Id,GroupId GroupId, double BasePrice):DomainEvent(Id);