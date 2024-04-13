using MediatR;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Quartz;
using TMS.Domain.Common.Models;
using TMS.Domain.OutBox;
using TMS.Infrastructure.Persistence;

namespace TMS.Infrastructure.BackGroundJobs;

[DisallowConcurrentExecution]
public class ProcessesOutBoxMessagesJob : IJob
{
    private readonly MainContext _dbContext;
    private readonly IPublisher _mediator;

    public ProcessesOutBoxMessagesJob(MainContext dbContext, IPublisher mediator)
    {
        _dbContext = dbContext;
        _mediator = mediator;
    }

    public async Task Execute(IJobExecutionContext context)
    {
        var messages = await _dbContext
            .OutboxMessages
            .Where(m => m.ProcessedOn == null)
            .OrderBy(m => m.RetryCount)
            .Take(20)
            .ToListAsync(context.CancellationToken);
        foreach (OutboxMessage outboxMessage in messages)
        {
            IntegrationEvent? domainEvent = JsonConvert.DeserializeObject<IntegrationEvent>(outboxMessage.Content, new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.All
            });
            if (domainEvent is null)
            {
                Console.WriteLine("Failed to deserialize domain event from outbox message with id {Id}",
                    outboxMessage.Id);
                continue;
            }

            try
            {
                await _mediator.Publish(domainEvent, context.CancellationToken);
                outboxMessage.ProcessedOn = DateTime.UtcNow;
            }
            catch (Exception e)
            {
                Console.WriteLine( $"Failed to process outbox message with id {outboxMessage.Id}" );
                outboxMessage.Error = e.Message;
                outboxMessage.RetryCount++;
            }
        }
        await _dbContext.SaveChangesAsync(context.CancellationToken);
    }
}