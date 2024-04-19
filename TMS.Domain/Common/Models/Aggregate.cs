using System.ComponentModel.DataAnnotations.Schema;

namespace TMS.Domain.Common.Models;

public abstract class Aggregate
{
    private readonly List<DomainEvent> _domainEvents = [];

    [NotMapped]
    public IEnumerable<DomainEvent> DomainEvents => _domainEvents;


    public void ClearDomainEvents()
    {
        _domainEvents.Clear();
    }

    public void RaiseDomainEvent(DomainEvent domainEvent)
    {
        _domainEvents.Add(domainEvent);
    }
}