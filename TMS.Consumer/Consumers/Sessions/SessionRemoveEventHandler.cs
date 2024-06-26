using MassTransit;
using TMS.Application.Common.Services;
using TMS.Domain.Common.Repositories;
using TMS.Domain.Teachers;
using TMS.MessagingContracts.Session;

namespace TMS.Consumer.Consumers.Sessions;

public class SessionRemoveEventHandler : IConsumer<SessionRemovedEvent>
{
    private readonly ISchedulerRepository _schedulerRepository;

    public SessionRemoveEventHandler(ISchedulerRepository schedulerRepository)
    {
        _schedulerRepository = schedulerRepository;
    }

    public async Task Consume(ConsumeContext<SessionRemovedEvent> context)
    {
        var scheduler = await _schedulerRepository.FirstOrDefaultAsync(x =>
            x.TeacherId == context.Message.TeacherId && x.Day == context.Message.Day && x.FiresOn == context.Message.EndTime);
        if (scheduler != null) _schedulerRepository.Remove(scheduler);
    }
}