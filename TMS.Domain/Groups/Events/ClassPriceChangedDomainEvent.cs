namespace TMS.Domain.Groups.Events;

public record ClassPriceChangedDomainEvent(Guid Id,GroupId GroupId, double BasePrice):DomainEvent(Id);