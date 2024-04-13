using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Newtonsoft.Json;
using TMS.Application.Common.Services;
using TMS.Domain.Common.Models;
using TMS.Domain.OutBox;

namespace TMS.Infrastructure.Persistence.interceptors;

public class ConvertDomainEventToOutBoxMessageInterceptor : SaveChangesInterceptor
{
    
    private readonly IDateTimeProvider _timeProvider ;
    private readonly IPublisher _publisher;

    public ConvertDomainEventToOutBoxMessageInterceptor(IDateTimeProvider timeProvider, IPublisher publisher)
    {
        _timeProvider = timeProvider;
        _publisher = publisher;
    }

    public override InterceptionResult<int> SavingChanges(DbContextEventData eventData, InterceptionResult<int> result)
    {
        ConvertIntegrationEventsToMessages(eventData.Context).GetAwaiter().GetResult();
        PublishingDomainEvents(eventData.Context).GetAwaiter().GetResult();
        return base.SavingChanges(eventData, result);
    }

    public override async ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData,
        InterceptionResult<int> result,
        CancellationToken cancellationToken = new CancellationToken())
    {
        await ConvertIntegrationEventsToMessages(eventData.Context);
        await PublishingDomainEvents(eventData.Context);
        return await base.SavingChangesAsync(eventData, result, cancellationToken);
    } 
    
    
    
    
    private async Task PublishingDomainEvents(DbContext? dbContext)
    {
        if (dbContext is null)
        {
            return ;
        }

        var domainEntities = dbContext.ChangeTracker.Entries<Aggregate>()
            .Where(x => x.Entity.DomainEvents.Any())
            .Select(x => x.Entity)
            .ToList();


        var domainEvents = domainEntities
            .SelectMany(e => e.DomainEvents)
            .ToList();

        foreach (var domainEvent in domainEvents)
        {
            await _publisher.Publish(domainEvent);
        }
        
    }


    private Task ConvertIntegrationEventsToMessages(DbContext? dbContext)
    {
        if (dbContext is null)
        {
            return Task.CompletedTask;
        }

        var domainEntities = dbContext.ChangeTracker.Entries<Aggregate>()
            .Where(x => x.Entity.IntegrationEvents.Any())
            .Select(x => x.Entity)
            .ToList();


        var outboxMessages = domainEntities
            .SelectMany(e => e.IntegrationEvents)
            .Select(domainEvent => new OutboxMessage()
            {
                Id = Guid.NewGuid(),
                OccurredOn = _timeProvider.Now,
                Type = domainEvent.GetType().Name,
                Content = JsonConvert.SerializeObject(
                    domainEvent,
                    new JsonSerializerSettings
                    {
                        TypeNameHandling = TypeNameHandling.All,
                    })
            })
            .ToList();
        
        dbContext.Set<OutboxMessage>().AddRange(outboxMessages);
        return Task.CompletedTask;
    }
}