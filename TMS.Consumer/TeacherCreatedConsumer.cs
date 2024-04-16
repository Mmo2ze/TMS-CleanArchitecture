using MassTransit;
using TMS.MessagingContracts;

namespace TMS.Consumer;

public class TeacherCreatedConsumer:IConsumer<TeacherCreatedEvent>
{
    public Task Consume(ConsumeContext<TeacherCreatedEvent> context)
    {
        var message = context.Message;
        Console.WriteLine($"Teacher created: {message.Name} with phone: {message.Phone}");
        return Task.CompletedTask;
    }
}