using MassTransit;
using TMS.Domain.Common.Repositories;
using TMS.MessagingContracts.Session;

namespace TMS.Consumer.Consumers.Sessions;

public class SessionGradeChangedEventHandler: IConsumer<SessionGradeChangedEvent>
{
    private readonly ISchedulerRepository _schedulerRepository;

    public SessionGradeChangedEventHandler(ISchedulerRepository schedulerRepository)
    {
        _schedulerRepository = schedulerRepository;
    }

    public async Task Consume(ConsumeContext<SessionGradeChangedEvent> context)
    {
        var message = context.Message;
        var scheduler = await _schedulerRepository.FirstOrDefaultAsync(x =>
            x.TeacherId == message.TeacherId && x.Grade == message.OldGrade && x.Day == message.Day &&
            x.FiresOn == message.EndTime);
        scheduler?.UpdateGrade(message.NewGrade);
    }
}