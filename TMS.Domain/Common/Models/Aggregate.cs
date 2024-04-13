using System.ComponentModel.DataAnnotations.Schema;

namespace TMS.Domain.Common.Models;

public abstract class Aggregate
{
    private readonly List<IntegrationEvent> _integrationEvents = [];
    private readonly List<DomainEvent> _domainEvents = [];

    [NotMapped]
    public IEnumerable<IntegrationEvent> IntegrationEvents => _integrationEvents;
    [NotMapped]
    public IEnumerable<DomainEvent> DomainEvents => _domainEvents;

    protected void RaiseIntegrationEvent(IntegrationEvent integrationEvent)
    {
        _integrationEvents.Add(integrationEvent);
    }

    public void ClearIntegrationEvents()
    {
        _integrationEvents.Clear();
    }

    public void ClearDomainEvents()
    {
        _domainEvents.Clear();
    }



    public void RemoveIntegrationEvent(Guid id)
    {
        var integrationEvent = _integrationEvents.FirstOrDefault(d => d.Id == id);
        if (integrationEvent != null) _integrationEvents.Remove(integrationEvent);
    }

    public void RaiseDomainEvent(DomainEvent domainEvent)
    {
        _domainEvents.Add(domainEvent);
    }
}