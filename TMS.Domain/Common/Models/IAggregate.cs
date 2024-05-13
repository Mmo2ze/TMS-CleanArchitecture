namespace TMS.Domain.Common.Models;

public interface IAggregate
{
    IEnumerable<DomainEvent> DomainEvents { get; }
    void ClearDomainEvents();
}